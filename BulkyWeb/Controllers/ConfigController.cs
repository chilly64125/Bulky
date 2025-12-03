using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpGet("app")]
        public IActionResult GetAppConfig()
        {
            var config = new
            {
                appName = "Bulky Vue Frontend",
                version = "1.0",
                apiBase = "/api"
            };

            return Ok(new { data = config });
        }
    }
}
