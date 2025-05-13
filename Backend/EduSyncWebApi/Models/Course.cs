using System;
using System.Collections.Generic;

namespace EduSyncWebApi.Models;

public partial class Course
{
    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? InstructorId { get; set; }

    public string? MediaUrl { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

    public virtual UserModel? Instructor { get; set; }
}
