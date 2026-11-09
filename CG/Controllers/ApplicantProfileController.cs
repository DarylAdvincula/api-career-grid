using CG.DTO.ApplicantProfile;
using CG.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/profile/applicant")]
    public class ApplicantProfileController : ControllerBase
    {
        private readonly IApplicantProfileService _applicantProfileService;
        private readonly IValidator<ApplicantProfileCreateDto> _applicantProfileCreateValidator;
        
        public ApplicantProfileController(
            IApplicantProfileService applicantProfileService,
            IValidator<ApplicantProfileCreateDto> applicantProfileCreateValidator
        )
        {
            _applicantProfileService = applicantProfileService;
            _applicantProfileCreateValidator = applicantProfileCreateValidator;
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicantProfileCreateDto request)
        {
            var result = await _applicantProfileCreateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                }));
            }

            return StatusCode(StatusCodes.Status201Created, await _applicantProfileService.CreateAsync(request));
        }
    }
}