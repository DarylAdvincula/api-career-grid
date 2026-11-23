using CG.Attributes;
using CG.DTO.Application;
using CG.Models;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/application")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [Employer(CompanyRole.HiringManager)]
        [HttpGet("job-posting/{jobPostingId}")]
        public async Task<IActionResult> Index(int jobPostingId)
        {
            return Ok(await _applicationService.GetAllByJobPostingIdAsync(jobPostingId));
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetAllForApplicant(ApplicationStatus? applicationStatus)
        {
            return Ok(await _applicationService.GetAllForApplicantAsync(applicationStatus));
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicationCreateDto request)
        {
            return StatusCode(StatusCodes.Status201Created, await _applicationService.CreateAsync(request));
        }

        [Authorize(Roles = "Applicant")]
        [HttpPatch("continue/{id}")]
        public async Task<IActionResult> ContinueApplication(int id)
        {
            return Ok(await _applicationService.ContinueApplicationAsync(id));
        }

        [Employer(CompanyRole.HiringManager)]
        [HttpPatch("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(
            int id, 
            ApplicationStatus? applicationStatus
        )
        {
            return Ok(await _applicationService.UpdateStatusAsync(id, applicationStatus));
        }

        [Authorize(Roles = "Applicant")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(
            int id, 
            [FromBody] ApplicationUpdateDto request
        )
        {
            return Ok(await _applicationService.UpdateAsync(id, request));
        }
    }
}