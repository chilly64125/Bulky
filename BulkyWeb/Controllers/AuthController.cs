using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BulkyBookWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public class LoginRequest
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
            {
                return BadRequest(new { message = "Email and password required" });
            }

            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, req.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // get roles for the user and normalize to short names the frontend expects
            var rawRoles = await _userManager.GetRolesAsync(user);
            var roles = rawRoles.Select(r =>
            {
                if (r.Contains("Admin")) return "Admin";
                if (r.Contains("Customer")) return "Customer";
                if (r.Contains("Company")) return "Company";
                if (r.Contains("Employee")) return "Employee";
                return r;
            }).ToList();

            // build a user DTO for response
            var userDto = new
            {
                userId = user.Id,
                email = user.Email,
                name = user.Name,
                roles = roles
            };

            // minimal token object (frontend expects token fields)
            var token = new
            {
                accessToken = string.Empty,
                refreshToken = string.Empty,
                expiresIn = 3600
            };

            return Ok(new { data = new { user = userDto, token = token } });
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> CurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var rawRoles = await _userManager.GetRolesAsync(user);
            var roles = rawRoles.Select(r =>
            {
                if (r.Contains("Admin")) return "Admin";
                if (r.Contains("Customer")) return "Customer";
                if (r.Contains("Company")) return "Company";
                if (r.Contains("Employee")) return "Employee";
                return r;
            }).ToList();
            var userDto = new
            {
                userId = user.Id,
                email = user.Email,
                name = user.Name,
                roles = roles
            };

            return Ok(new { data = userDto });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Signed out" });
        }
    }
}
