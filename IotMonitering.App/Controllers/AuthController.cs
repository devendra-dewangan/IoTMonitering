using IoTMonitoring.App.Services;
using IoTMonitoring.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IoTMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userAuthService;

        public AuthController(IAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            var token = await _userAuthService.AuthenticateUser(request);

            if (string.IsNullOrEmpty(token.Item1))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new UserLoginResponseDto { Token = token.Item1, RefreshToken = token.Item2 });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            var user = await _userAuthService.RegisterUser(request);

            if (user == null)
            {
                return BadRequest(new { message = "Failed to register user" });
            }

            return Ok(new UserRegisterResponseDto()
            {
                Message = "User registered successfully"
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshRequestDto request)
        {
            var user = await _userAuthService.GetToken(request.RefreshToken);
            return Ok(new UserLoginResponseDto { Token = user.Item1, RefreshToken = user.Item2 });

        }
    }
}