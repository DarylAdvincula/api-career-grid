using CG.Models;

namespace CG.DTO.ApplicationStatusLog
{
    public class ApplicationStatusLogCreateDto
    {
        public int ApplicationId { get; set; }
        public int ChangedByUserId { get; set; }
        public ApplicationStatus NewApplicationStatus { get; set; }
        public string? Notes { get; set; }
    }
}