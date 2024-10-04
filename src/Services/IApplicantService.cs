using EmploymentSystem.Models;
using EmploymentSystem.RTOs;

namespace EmploymentSystem.Services
{
    public interface IApplicantService
    {
        Task<List<Vacancy>> SearchVacancies(string keyword);
        Task ApplyForVacancy(int vacancyId, int applicantId);
        Task<bool> CanApplyForVacancy(int applicantId);
        Task<List<VacancyRTO>> GetAppliedVacancies(int applicantId);
    }

}