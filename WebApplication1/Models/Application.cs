namespace EmploymentSystem.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int VacancyId { get; set; }
        public required Vacancy Vacancy { get; set; }
        public int ApplicantId { get; set; }
        public required User Applicant { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
