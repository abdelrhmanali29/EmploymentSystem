using System.Text.Json.Serialization;

namespace EmploymentSystem.Models
{
    public class Vacancy
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int MaxApplications { get; set; }
        public int ApplicationsCount { get; set; }  
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int EmployerId { get; set; }
        public required User Employer { get; set; }

        
        [JsonIgnore]
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
