using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBookWeb.Controllers.Api
{
    [ApiController]
    [Route("api/category")]
    [Authorize]
    public class CategoryApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = _unitOfWork.Category.GetAll();
                return Ok(new { data = categories });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return NotFound(new { message = "Category not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Category data)
        {
            try
            {
                if (data == null)
                    return BadRequest(new { message = "Data is required" });

                _unitOfWork.Category.Add(data);
                _unitOfWork.Save();
                return Ok(new { message = "Category created", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Category data)
        {
            try
            {
                if (data == null || data.Id != id)
                    return BadRequest(new { message = "Invalid data" });

                _unitOfWork.Category.Update(data);
                _unitOfWork.Save();
                return Ok(new { message = "Category updated", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = _unitOfWork.Category.Get(c => c.Id == id);
                if (category == null)
                    return NotFound(new { message = "Category not found" });

                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
                return Ok(new { message = "Category deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    [ApiController]
    [Route("api/company")]
    [Authorize]
    public class CompanyApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var companies = _unitOfWork.Company.GetAll();
                return Ok(new { data = companies });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var company = _unitOfWork.Company.Get(c => c.Id == id);
                if (company == null)
                    return NotFound(new { message = "Company not found" });
                return Ok(new { data = company });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Company data)
        {
            try
            {
                if (data == null)
                    return BadRequest(new { message = "Data is required" });

                _unitOfWork.Company.Add(data);
                _unitOfWork.Save();
                return Ok(new { message = "Company created", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Company data)
        {
            try
            {
                if (data == null || data.Id != id)
                    return BadRequest(new { message = "Invalid data" });

                _unitOfWork.Company.Update(data);
                _unitOfWork.Save();
                return Ok(new { message = "Company updated", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var company = _unitOfWork.Company.Get(c => c.Id == id);
                if (company == null)
                    return NotFound(new { message = "Company not found" });

                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();
                return Ok(new { message = "Company deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    [ApiController]
    [Route("api/product")]
    [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = _unitOfWork.Product.GetAll();
                return Ok(new { data = products });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(p => p.Id == id);
                if (product == null)
                    return NotFound(new { message = "Product not found" });
                return Ok(new { data = product });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Product data)
        {
            try
            {
                if (data == null)
                    return BadRequest(new { message = "Data is required" });

                _unitOfWork.Product.Add(data);
                _unitOfWork.Save();
                return Ok(new { message = "Product created", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Product data)
        {
            try
            {
                if (data == null || data.Id != id)
                    return BadRequest(new { message = "Invalid data" });

                _unitOfWork.Product.Update(data);
                _unitOfWork.Save();
                return Ok(new { message = "Product updated", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(p => p.Id == id);
                if (product == null)
                    return NotFound(new { message = "Product not found" });

                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return Ok(new { message = "Product deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = _unitOfWork.ApplicationUser.GetAll();
                return Ok(new { data = users });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
                if (user == null)
                    return NotFound(new { message = "User not found" });
                return Ok(new { data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ApplicationUser data)
        {
            try
            {
                if (data == null)
                    return BadRequest(new { message = "Data is required" });

                _unitOfWork.ApplicationUser.Add(data);
                _unitOfWork.Save();
                return Ok(new { message = "User created", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationUser data)
        {
            try
            {
                if (data == null || data.Id != id)
                    return BadRequest(new { message = "Invalid data" });

                _unitOfWork.ApplicationUser.Update(data);
                _unitOfWork.Save();
                return Ok(new { message = "User updated", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                _unitOfWork.ApplicationUser.Remove(user);
                _unitOfWork.Save();
                return Ok(new { message = "User deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    [ApiController]
    [Route("api/order")]
    [Authorize]
    public class OrderApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = _unitOfWork.OrderHeader.GetAll();
                return Ok(new { data = orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = _unitOfWork.OrderHeader.Get(o => o.Id == id);
                if (order == null)
                    return NotFound(new { message = "Order not found" });
                return Ok(new { data = order });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderHeader data)
        {
            try
            {
                if (data == null)
                    return BadRequest(new { message = "Data is required" });

                _unitOfWork.OrderHeader.Add(data);
                _unitOfWork.Save();
                return Ok(new { message = "Order created", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderHeader data)
        {
            try
            {
                if (data == null || data.Id != id)
                    return BadRequest(new { message = "Invalid data" });

                _unitOfWork.OrderHeader.Update(data);
                _unitOfWork.Save();
                return Ok(new { message = "Order updated", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = _unitOfWork.OrderHeader.Get(o => o.Id == id);
                if (order == null)
                    return NotFound(new { message = "Order not found" });

                _unitOfWork.OrderHeader.Remove(order);
                _unitOfWork.Save();
                return Ok(new { message = "Order deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
