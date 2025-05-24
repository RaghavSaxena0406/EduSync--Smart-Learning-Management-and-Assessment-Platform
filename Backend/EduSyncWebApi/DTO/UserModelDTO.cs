namespace EduSyncWebApi.DTO
{
    public class UserModelDTO
    {
        public Guid UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public string PasswordHash { get; set; } = null!;
    }
}
