using CG.DTO.UserAccount;
using CG.Services.Interfaces;
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

        [HttpPost("register-as-applicant")]
        public async Task<IActionResult> RegisterAsApplicant([FromBody] UserAccountCreateDto request)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                await _authService.RegisterAsApplicantAsync(request)
            );
        }
    }
}