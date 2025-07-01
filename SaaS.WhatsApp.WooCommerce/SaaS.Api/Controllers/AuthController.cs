using Microsoft.AspNetCore.Mvc;
using SaaS.Application.Dtos.Auth;
using SaaS.Application.IServices.Auth;

namespace SaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _authService.RegisterAsync(dto);
                    return Ok(response);
                }
                return BadRequest("Invalid registration attempt. Please check your details and try again.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _authService.LoginAsync(loginDto);
                    return Ok(response);
                }
                return BadRequest("Invalid login attempt. Please check your credentials and try again.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }  
    }
}
