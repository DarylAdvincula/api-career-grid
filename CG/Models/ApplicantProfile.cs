using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public class ApplicantProfile
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }

        [StringLength(150)]
        public string? Headline { get; set; }
        
        [StringLength(150)]
        public string? HomeLocation { get; set; }

        [StringLength(1500)] // instead of nvarchar(max)
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public UserAccount UserAccount { get; set; } = null!;
    }
}