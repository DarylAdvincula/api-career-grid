namespace CG.Models
{
    public class ApplicationStatusLog
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ChangedByUserId { get; set; }
        public ApplicationStatus? OldApplicationStatus { get; set; }
        public ApplicationStatus NewApplicationStatus { get; set; }
        public string? Notes { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        public Application Application { get; set; } = null!;
        public UserAccount ChangedByUser { get; set; } = null!;

        // +1 index (non-clustered)
        // [x] ApplicationID
    }
}