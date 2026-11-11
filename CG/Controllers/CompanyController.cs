using CG.Models;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/company")]
    public class CompanyController : ControllerBase
    {
        ICompanyService _companyService;
        
        public CompanyController(
            ICompanyService companyService
        )
        {
            _companyService = companyService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index(string? search, VerificationStatus? verificationStatus)
        {
            return Ok(await _companyService.GetAllAsync(search, verificationStatus));
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApproved(string? search)
        {
            return Ok(await _companyService.GetAllAsync(search, VerificationStatus.Approved));
        }

        // needs mailing
        [Authorize(Roles = "Admin")]
        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            return Ok(await _companyService.ApproveAsync(id));
        }

        // needs mailing
        [Authorize(Roles = "Admin")]
        [HttpPatch("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            return Ok(await _companyService.RejectAsync(id));
        }
    }
}