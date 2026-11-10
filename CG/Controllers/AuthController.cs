using CG.DTO.Company;
using CG.DTO.UserAccount;
using CG.Helpers.Interfaces;
using CG.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly IAuthService _authService;
        private readonly IValidator<UserAccountCreateDto> _applicantCreateValidator;
        private readonly IValidator<CompanyCreateDto> _companyCreateValidator;
        private readonly IValidator<CompanyJoinDto> _companyJoinValidator;

        public AuthController(
            ISystemConfiguration systemConfiguration,
            IAuthService authService,
            IValidator<UserAccountCreateDto> applicantCreateValidator,
            IValidator<CompanyCreateDto> companyCreateValidator,
            IValidator<CompanyJoinDto> companyJoinValidator
        )
        {
            _systemConfiguration = systemConfiguration;
            _authService = authService;
            _applicantCreateValidator = applicantCreateValidator;
            _companyCreateValidator = companyCreateValidator;
            _companyJoinValidator = companyJoinValidator;
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
            var result = await _applicantCreateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                }));
            }

            return StatusCode(StatusCodes.Status201Created, await _authService.RegisterAsApplicant(request));
        }

        [HttpPost("register/owner")]
        public async Task<IActionResult> RegisterAsOwner([FromBody] CompanyCreateDto request)
        {
            var result = await _companyCreateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                }));
            }
            
            return StatusCode(StatusCodes.Status201Created, await _authService.RegisterAsOwner(request));
        }

        [HttpPost("register/staff")]
        public async Task<IActionResult> RegisterAsStaff([FromBody] CompanyJoinDto request)
        {
            var result = await _companyJoinValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                }));
            }

            return StatusCode(StatusCodes.Status201Created, await _authService.RegisterAsStaff(request));
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
            return RedirectPermanent(_systemConfiguration.AppFrontendUrl + "/login?verified=true");
        }
    }
}