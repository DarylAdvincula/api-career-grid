using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public class JobSubClassification
    {
        public int Id { get; set; }
        public int JobClassificationId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public JobClassification JobClassification { get; set; } = null!;

        // +1 constraint
        // [x] Unique (Id, JobClassificationId) together
    }
}