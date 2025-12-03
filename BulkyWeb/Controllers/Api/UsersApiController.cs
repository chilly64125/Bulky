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
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = SD.Role_Admin)]
    public class UsersApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UsersApiController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET /api/admin/users
        /// Returns a list of all users with their roles
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<List<UserRoleDto>>> GetUsers()
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
                        UserName = user.UserName ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        Roles = roles.ToList()
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve users", details = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/admin/users/{userId}
        /// Returns a specific user with their roles
        /// </summary>
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<UserRoleDto>> GetUser(string userId)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                var roles = await _userManager.GetRolesAsync(user);
                var result = new UserRoleDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Roles = roles.ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve user", details = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/admin/users/{userId}/roles
        /// Update a user's roles
        /// </summary>
        [HttpPut("users/{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string userId, [FromBody] UpdateRolesRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { error = "User ID is required" });
                }

                if (request?.Roles == null)
                {
                    return BadRequest(new { error = "Roles list is required" });
                }

                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                // Get current roles
                var currentRoles = (await _userManager.GetRolesAsync(user)).ToList();

                // Remove roles that are no longer in the request
                foreach (var role in currentRoles)
                {
                    if (!request.Roles.Contains(role))
                    {
                        var removeResult = await _userManager.RemoveFromRoleAsync(user, role);
                        if (!removeResult.Succeeded)
                        {
                            return StatusCode(500, new { error = $"Failed to remove role {role}", details = string.Join(", ", removeResult.Errors.Select(e => e.Description)) });
                        }
                    }
                }

                // Add new roles
                foreach (var role in request.Roles)
                {
                    if (!currentRoles.Contains(role))
                    {
                        // Ensure role exists
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                            if (!createRoleResult.Succeeded)
                            {
                                return StatusCode(500, new { error = $"Failed to create role {role}", details = string.Join(", ", createRoleResult.Errors.Select(e => e.Description)) });
                            }
                        }

                        var addResult = await _userManager.AddToRoleAsync(user, role);
                        if (!addResult.Succeeded)
                        {
                            return StatusCode(500, new { error = $"Failed to add role {role}", details = string.Join(", ", addResult.Errors.Select(e => e.Description)) });
                        }
                    }
                }

                return Ok(new { message = "User roles updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to update user roles", details = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/admin/users/{userId}
        /// Delete a user
        /// </summary>
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { error = "User ID is required" });
                }

                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                // Prevent deleting the current logged-in user
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userId == currentUserId)
                {
                    return BadRequest(new { error = "Cannot delete the currently logged-in user" });
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(500, new { error = "Failed to delete user", details = string.Join(", ", result.Errors.Select(e => e.Description)) });
                }

                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to delete user", details = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/admin/dashboard
        /// Returns dashboard statistics
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
        {
            try
            {
                var users = _unitOfWork.ApplicationUser.GetAll().ToList();
                var stats = new DashboardStatsDto
                {
                    TotalUsers = users.Count
                };

                // Count users by role
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        if (role == SD.Role_Admin)
                            stats.AdminCount++;
                        else if (role == SD.Role_Customer)
                            stats.CustomerCount++;
                        else if (role == SD.Role_Employee)
                            stats.EmployeeCount++;
                        else if (role == SD.Role_Company)
                            stats.CompanyCount++;
                        else
                            stats.OtherRolesCount++;
                    }
                }

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve dashboard stats", details = ex.Message });
            }
        }
    }

    /// <summary>
    /// DTO for user with roles
    /// </summary>
    public class UserRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// Request body for updating user roles
    /// </summary>
    public class UpdateRolesRequest
    {
        public List<string> Roles { get; set; } = new List<string>();
    }
}
