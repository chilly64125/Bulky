using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBookWeb.Controllers.Api
{
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

        /// <summary>
        /// Get all companies
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var companies = _unitOfWork.Company.GetAll().ToList();
                var paginatedCompanies = companies
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = paginatedCompanies,
                    total = companies.Count,
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
        /// Get company by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var company = _unitOfWork.Company.Get(u => u.Id == id);
                if (company == null)
                    return NotFound(new { success = false, message = "Company not found" });

                return Ok(new { success = true, data = company });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Create new company (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] Company company)
        {
            try
            {
                if (company == null)
                    return BadRequest(new { success = false, message = "Company data is required" });

                if (string.IsNullOrEmpty(company.Name))
                    return BadRequest(new { success = false, message = "Company name is required" });

                _unitOfWork.Company.Add(company);
                _unitOfWork.Save();

                return Ok(new { success = true, data = company, message = "Company created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update company (admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] Company company)
        {
            try
            {
                if (company == null)
                    return BadRequest(new { success = false, message = "Company data is required" });

                var existingCompany = _unitOfWork.Company.Get(u => u.Id == id);
                if (existingCompany == null)
                    return NotFound(new { success = false, message = "Company not found" });

                existingCompany.Name = company.Name;
                existingCompany.StreetAddress = company.StreetAddress;
                existingCompany.City = company.City;
                existingCompany.State = company.State;
                existingCompany.PostalCode = company.PostalCode;
                existingCompany.PhoneNumber = company.PhoneNumber;

                _unitOfWork.Company.Update(existingCompany);
                _unitOfWork.Save();

                return Ok(new { success = true, data = existingCompany, message = "Company updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete company (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var company = _unitOfWork.Company.Get(u => u.Id == id);
                if (company == null)
                    return NotFound(new { success = false, message = "Company not found" });

                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Company deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
