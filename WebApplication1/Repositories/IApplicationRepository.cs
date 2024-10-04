using EmploymentSystem.Models;

namespace EmploymentSystem.Repositories
{
    public interface IApplicationRepository
    {
        Task<List<Application>> GetByVacancyIdAsync(int vacancyId);
        Task<Application> GetByApplicantIdAndVacancyIdAsync(int applicantId, int vacancyId);
        Task AddAsync(Application application);
        Task<List<Application>> GetByApplicantIdAsync(int applicantId);
    }

}
