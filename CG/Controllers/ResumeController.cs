using CG.DTO.Resume;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CG.Controllers
{
    [ApiController]
    [Route("/api/resume")]
    public class ResumeController : ControllerBase
    {        
        private readonly IResumeService _resumeService;
        
        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _resumeService.GetAllFromCurrentUserAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] ResumeCreateDto request)
        {
            return StatusCode(StatusCodes.Status201Created, await _resumeService.CreateAsync(request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resumeService.DeleteAsync(id);
            return NoContent();
        }
    }
}