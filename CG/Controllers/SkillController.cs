using CG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/skill")]
    public class SkillController : ControllerBase
    {
        ISkillService _skillService;
        
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? search)
        {
            return Ok(await _skillService.GetAllAsync(search));
        }
    }
}