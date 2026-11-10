using CG.Attributes;
using CG.DTO.JobPosting;
using CG.Models;
using CG.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/job-posting")]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingService _jobPostingService;
        private readonly IValidator<JobPostingCreateDto> _jobPostingCreateValidator;
        
        public JobPostingController(
            IJobPostingService jobPostingService,
            IValidator<JobPostingCreateDto> jobPostingCreateValidator
        )
        {
            _jobPostingService = jobPostingService;
            _jobPostingCreateValidator = jobPostingCreateValidator;
        }

        [Authorize(Roles = "Applicant,Employer")]
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] string? search,
            [FromQuery] ApprovalStatus? approvalStatus,
            [FromQuery] bool? active
        )
        {
            return Ok(await _jobPostingService.GetAllAsync(search, approvalStatus, active));
        }
        
        [Authorize(Roles = "Employer")]
        [Employer(CompanyRole.Recruiter)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobPostingCreateDto request)
        {
            var result = await _jobPostingCreateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                }));
            }

            return StatusCode(StatusCodes.Status201Created, await _jobPostingService.CreateAsync(request));
        }

        [Employer(CompanyRole.Recruiter)]
        [HttpPatch("mark-for-approval/{id}")]
        public async Task<IActionResult> MarkForApproval(int id)
        {
            return Ok(await _jobPostingService.MarkForApproval(id));
        }

        [Employer(CompanyRole.Admin)]
        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {   
            return Ok(await _jobPostingService.ApproveAsync(id));
        }

        [Employer(CompanyRole.Admin)]
        [HttpPatch("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {   
            return Ok(await _jobPostingService.RejectAsync(id));
        }

        [Employer(CompanyRole.Admin)]
        [HttpDelete("archive/{id}")]
        public async Task<IActionResult> Archive(int id)
        {   
            return Ok(await _jobPostingService.ArchiveAsync(id));
        }

        [Employer(CompanyRole.Admin)]
        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {   
            return Ok(await _jobPostingService.RestoreAsync(id));
        }
    }
}