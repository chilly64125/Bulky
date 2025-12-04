using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBookWeb.Controllers.Api
{
    [ApiController]
    [Route("api/product")]
    [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductApiController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var products = _unitOfWork.Product.GetAll(includeProperties: "Category,Company,ProductImages").ToList();
                var paginatedProducts = products
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = paginatedProducts,
                    total = products.Count,
                    page = page,
                    pageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category,Company,ProductImages");
                if (product == null)
                    return NotFound(new { success = false, message = "Product not found" });

                return Ok(new { success = true, data = product });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Create new product (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var product = new Product
                {
                    Title = Request.Form["title"],
                    Description = Request.Form["description"],
                    ISBN = Request.Form["isbn"],
                    CategoryId = int.Parse(Request.Form["categoryId"]),
                    CompanyId = int.Parse(Request.Form["companyId"]),
                    HDate = Request.Form["hDate"],
                    HeldYN = string.IsNullOrEmpty(Request.Form["heldYN"].ToString()) ? 'N' : Request.Form["heldYN"].ToString()[0],
                    ListPrice = double.Parse(Request.Form["listPrice"])
                };

                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();

                // Handle file uploads
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string productPath = Path.Combine(wwwRootPath, @"images\products\product-" + product.Id);
                            string finalPath = Path.Combine(productPath, fileName);

                            if (!Directory.Exists(productPath))
                                Directory.CreateDirectory(productPath);

                            using (var fileStream = new FileStream(finalPath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            ProductImage productImage = new()
                            {
                                ImageUrl = @"\images\products\product-" + product.Id + @"\" + fileName,
                                ProductId = product.Id,
                            };

                            product.ProductImages.Add(productImage);
                        }
                    }
                    _unitOfWork.Product.Update(product);
                    _unitOfWork.Save();
                }

                return Ok(new { success = true, data = product, message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update product (admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                if (product == null)
                    return NotFound(new { success = false, message = "Product not found" });

                product.Title = Request.Form["title"];
                product.Description = Request.Form["description"];
                product.ISBN = Request.Form["isbn"];
                product.CategoryId = int.Parse(Request.Form["categoryId"]);
                product.CompanyId = int.Parse(Request.Form["companyId"]);
                product.HDate = Request.Form["hDate"];
                product.HeldYN = string.IsNullOrEmpty(Request.Form["heldYN"].ToString()) ? 'N' : Request.Form["heldYN"].ToString()[0];
                product.ListPrice = double.Parse(Request.Form["listPrice"]);

                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();

                // Handle file uploads
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string productPath = Path.Combine(wwwRootPath, @"images\products\product-" + product.Id);
                            string finalPath = Path.Combine(productPath, fileName);

                            if (!Directory.Exists(productPath))
                                Directory.CreateDirectory(productPath);

                            using (var fileStream = new FileStream(finalPath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            ProductImage productImage = new()
                            {
                                ImageUrl = @"\images\products\product-" + product.Id + @"\" + fileName,
                                ProductId = product.Id,
                            };

                            product.ProductImages.Add(productImage);
                        }
                    }
                    _unitOfWork.Product.Update(product);
                    _unitOfWork.Save();
                }

                return Ok(new { success = true, data = product, message = "Product updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete product (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                if (product == null)
                    return NotFound(new { success = false, message = "Product not found" });

                // Delete images from file system
                if (product.ProductImages != null && product.ProductImages.Count > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    foreach (var image in product.ProductImages)
                    {
                        string imagePath = Path.Combine(wwwRootPath, image.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                }

                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete product image (admin only)
        /// </summary>
        [HttpDelete("image/{imageId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteImage(int imageId)
        {
            try
            {
                var productImage = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
                if (productImage == null)
                    return NotFound(new { success = false, message = "Image not found" });

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imagePath = Path.Combine(wwwRootPath, productImage.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _unitOfWork.ProductImage.Remove(productImage);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Image deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
