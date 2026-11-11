using CG.Models;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/staff")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, CompanyRole? companyRole)
        {
            return Ok(await _staffService.GetAll(search, companyRole));
        }

        // needs mailing
        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            return Ok(await _staffService.Approve(id));
        }

        // needs mailing
        [HttpPatch("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            return Ok(await _staffService.Reject(id));
        }
    }
}