using System;
using EduSyncWebApi.Models;

namespace EduSyncWebApi.DTOs
{
    public class AssessmentResultDTO
    {
        public Guid ResultId { get; set; }
        public Guid AssessmentId { get; set; }
        public Guid StudentId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? Answers { get; set; }

        public string? AssessmentTitle { get; set; } // ✅ Added

        public AssessmentResultDTO(AssessmentResult result)
        {
            ResultId = result.ResultId;
            AssessmentId = result.AssessmentId;
            StudentId = result.StudentId;
            Score = result.Score;
            MaxScore = result.MaxScore;
            SubmissionDate = result.SubmissionDate;
            Answers = result.Answers;

            AssessmentTitle = result.Assessment?.Title; // ✅ Pull title if available
        }
    }

    public class CreateAssessmentResultDTO
    {
        public Guid AssessmentId { get; set; }
        public Guid StudentId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public string? Answers { get; set; }
    }

    public class UpdateAssessmentResultDTO
    {
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public string? Answers { get; set; }
    }
}
