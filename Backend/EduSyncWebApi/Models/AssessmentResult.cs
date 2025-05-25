using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSyncWebApi.Models
{
    public class AssessmentResult
    {
        [Key]
        public Guid ResultId { get; set; }

        [Required]
        public Guid AssessmentId { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public int MaxScore { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; }

        public string? Answers { get; set; }

        // Navigation properties
        [ForeignKey("AssessmentId")]
        public virtual Assessment? Assessment { get; set; }

        [ForeignKey("StudentId")]
        public virtual UserModel? Student { get; set; }
    }
}