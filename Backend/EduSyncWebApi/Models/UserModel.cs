using System;
using System.Collections.Generic;

namespace EduSyncWebApi.Models;

public partial class UserModel
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
