using CG.Models;

namespace CG.DTO.ApplicationStatusLog
{
    public class ApplicationStatusLogUpdateDto
    {
        public ApplicationStatus NewApplicationStatus { get; set; }
        public string? Notes { get; set; }
    }
}