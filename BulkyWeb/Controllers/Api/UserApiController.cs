using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using BulkyBookWeb.Controllers.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBookWeb.Controllers.Api
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserApiController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET /api/user
        /// Returns all users with their roles
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = _unitOfWork.ApplicationUser.GetAll().ToList();
                var result = new List<UserRoleDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    result.Add(new UserRoleDto
                    {
                        Id = user.Id,
                        Email = user.Email ?? "",
                        Name = user.Name ?? "",
                        PhoneNumber = user.PhoneNumber ?? "",
                        Roles = roles.ToList()
                    });
                }

                var pagedResult = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        items = pagedResult,
                        totalCount = result.Count,
                        pageNumber,
                        pageSize,
                        totalPages = (result.Count + pageSize - 1) / pageSize
                    },
                    message = "Users retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/user/{id}
        /// Returns a specific user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetById(string id)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
                if (user == null)
                    return NotFound(new { success = false, message = "User not found" });

                var roles = await _userManager.GetRolesAsync(user);
                var result = new UserRoleDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Name = user.Name ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    Roles = roles.ToList()
                };

                return Ok(new { success = true, data = result, message = "User retrieved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/user
        /// Creates a new user (typically for admin user creation)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<dynamic>> Create([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid user data", errors = ModelState.Values.SelectMany(v => v.Errors) });

                var user = new ApplicationUser
                {
                    UserName = createUserDto.Email,
                    Email = createUserDto.Email,
                    Name = createUserDto.Name,
                    PhoneNumber = createUserDto.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!result.Succeeded)
                    return BadRequest(new { success = false, message = "User creation failed", errors = result.Errors.Select(e => e.Description) });

                // Assign default role
                if (!string.IsNullOrEmpty(createUserDto.Role))
                {
                    await _userManager.AddToRoleAsync(user, createUserDto.Role);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserRoleDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Name = user.Name ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    Roles = roles.ToList()
                };

                return CreatedAtAction(nameof(GetById), new { id = user.Id },
                    new { success = true, data = userDto, message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/user/{id}
        /// Updates an existing user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> Update(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
                if (user == null)
                    return NotFound(new { success = false, message = "User not found" });

                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid user data", errors = ModelState.Values.SelectMany(v => v.Errors) });

                user.Email = updateUserDto.Email ?? user.Email;
                user.Name = updateUserDto.Name ?? user.Name;
                user.PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return BadRequest(new { success = false, message = "User update failed", errors = result.Errors.Select(e => e.Description) });

                // Update roles if provided
                if (updateUserDto.Roles != null && updateUserDto.Roles.Any())
                {
                    var existingRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, existingRoles);
                    await _userManager.AddToRolesAsync(user, updateUserDto.Roles);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserRoleDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Name = user.Name ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    Roles = roles.ToList()
                };

                return Ok(new { success = true, data = userDto, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/user/{id}
        /// Deletes a user by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> Delete(string id)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
                if (user == null)
                    return NotFound(new { success = false, message = "User not found" });

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return BadRequest(new { success = false, message = "User deletion failed", errors = result.Errors.Select(e => e.Description) });

                return Ok(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

