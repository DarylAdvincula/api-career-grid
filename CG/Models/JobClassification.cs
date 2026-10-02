using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public class JobClassification
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;
    }
}