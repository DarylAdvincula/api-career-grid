using CG.DTO.ApplicantProfile;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/profile/applicant")]
    public class ApplicantProfileController : ControllerBase
    {
        private readonly IApplicantProfileService _applicantProfileService;
        
        public ApplicantProfileController(IApplicantProfileService applicantProfileService)
        {
            _applicantProfileService = applicantProfileService;
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicantProfileCreateDto request)
        {
            return StatusCode(StatusCodes.Status201Created, await _applicantProfileService.CreateAsync(request));
        }
    }
}