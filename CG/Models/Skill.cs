using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public int? JobClassificationId { get; set; } // [x] set null on delete
        public int? JobSubClassificationId { get; set; } // [x] do nothing on delete

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public JobClassification? JobClassification { get; set; } = null!;
        public JobSubClassification? JobSubClassification { get; set; } = null!;

        // +3 indexes (non-clustered)
        // [x] Name
        // [x] JobClassificationId
        // [x] JobSubClassificationId
    }
}