using CG.Models;

namespace CG.DTO.ApplicationStatusLog
{
    public class ApplicationStatusLogDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ChangedByUserId { get; set; }
        public ApplicationStatus? OldApplicationStatus { get; set; }
        public ApplicationStatus NewApplicationStatus { get; set; }
        public string? Notes { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}