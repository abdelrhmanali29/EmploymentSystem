using EmploymentSystem.Models;

namespace EmploymentSystem.RTOs
{
    public class ApplicationRTO
    {
        public int Id { get; set; }
        public int VacancyId { get; set; }
        public int ApplicantId { get; set; }
        public required User Applicant { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
