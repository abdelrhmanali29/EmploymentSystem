using EmploymentSystem.DTOs;
using EmploymentSystem.Models;
using EmploymentSystem.RTOs;

namespace EmploymentSystem.Services
{
    public interface IEmployerService
    {
        Task CreateVacancy(VacancyDTO vacancyDto, int employerId);
        Task UpdateVacancy(int vacancyId, VacancyDTO vacancyDto);
        Task DeleteVacancy(int vacancyId);
        Task<List<Vacancy>> GetEmployerVacancies(int employerId);
        Task<Vacancy> GetVacancyById(int vacancyId);
        Task<List<Application>> GetVacancyApplications(int vacancyId);
        Task ArchiveExpiredVacancies();
    }

}