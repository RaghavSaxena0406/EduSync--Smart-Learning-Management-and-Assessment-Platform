namespace EduSyncWebApi.DTO
{
    public class RegisterDTO
    {
        public required string Name { get; set; } = null!;
        public required string Email { get; set; } = null!;
        public required string Role { get; set; }
        public required string PasswordHash { get; set; } = null!;
    }
}
