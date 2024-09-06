
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (_loginService.ValidateUser(request.Email, request.Password))
            {
                var token = _loginService.GenerateJwtToken(request.Email);
                return Ok(new { Token = token, Message = "Login successful" });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }
    }
}
