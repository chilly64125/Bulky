using System.Collections.Generic;

namespace BulkyBookWeb.Controllers.Api.Dtos
{
    /// <summary>
    /// DTO for user with roles
    /// </summary>
    public class UserRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// DTO for creating a new user
    /// </summary>
    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for updating a user
    /// </summary>
    public class UpdateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<string>? Roles { get; set; }
    }
}
