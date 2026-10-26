using CG.DTO.Company;
using CG.DTO.UserAccount;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            return Ok(await _authService.MeAsync());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAccountCreateDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.Register(request)
            );
        }

        [HttpPost("create-company")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.CreateCompany(request)
            );
        }

        [HttpPost("join-company")]
        public async Task<IActionResult> JoinCompany([FromBody] CompanyJoinDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.JoinCompany(request)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAccountLoginDto request)
        {
            await _authService.LoginAsync(request);
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }
    }
}