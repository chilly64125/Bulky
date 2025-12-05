using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var categories = _unitOfWork.Category.GetAll().OrderBy(c => c.DisplayOrder).ToList();
                var paginatedCategories = categories
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = paginatedCategories,
                    total = categories.Count,
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
        /// Get category by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var category = _unitOfWork.Category.Get(u => u.Id == id);
                if (category == null)
                    return NotFound(new { success = false, message = "Category not found" });

                return Ok(new { success = true, data = category });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Create new category (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] Category category)
        {
            try
            {
                if (category == null)
                    return BadRequest(new { success = false, message = "Category data is required" });

                if (string.IsNullOrEmpty(category.Name))
                    return BadRequest(new { success = false, message = "Category name is required" });

                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();

                return Ok(new { success = true, data = category, message = "Category created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update category (admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            try
            {
                if (category == null)
                    return BadRequest(new { success = false, message = "Category data is required" });

                var existingCategory = _unitOfWork.Category.Get(u => u.Id == id);
                if (existingCategory == null)
                    return NotFound(new { success = false, message = "Category not found" });

                existingCategory.Name = category.Name;
                existingCategory.DisplayOrder = category.DisplayOrder;

                _unitOfWork.Category.Update(existingCategory);
                _unitOfWork.Save();

                return Ok(new { success = true, data = existingCategory, message = "Category updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete category (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = _unitOfWork.Category.Get(u => u.Id == id);
                if (category == null)
                    return NotFound(new { success = false, message = "Category not found" });

                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
