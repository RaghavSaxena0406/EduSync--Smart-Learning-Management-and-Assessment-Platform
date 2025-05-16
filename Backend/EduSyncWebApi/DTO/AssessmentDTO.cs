namespace EduSyncWebApi.DTO
{
    public class AssessmentDTO
    {
        public Guid AssessmentId { get; set; }

        public Guid? CourseId { get; set; }

        public string Title { get; set; } = null!;

        public string? Questions { get; set; }

        public int? MaxScore { get; set; }
    }
}
