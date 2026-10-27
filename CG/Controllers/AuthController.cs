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

        [HttpPost("register/applicant")]
        public async Task<IActionResult> RegisterAsApplicant([FromBody] UserAccountCreateDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.RegisterAsApplicant(request)
            );
        }

        [HttpPost("register/owner")]
        public async Task<IActionResult> RegisterAsOwner([FromBody] CompanyCreateDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.RegisterAsOwner(request)
            );
        }

        [HttpPost("register/staff")]
        public async Task<IActionResult> RegisterAsStaff([FromBody] CompanyJoinDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.RegisterAsStaff(request)
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

        [HttpGet("verify/email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string code)
        {
            await _authService.VerifyEmailAsync(code);
            return Ok();
        }
    }
}