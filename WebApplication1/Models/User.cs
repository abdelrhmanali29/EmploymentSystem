namespace EmploymentSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
