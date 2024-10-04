using EmploymentSystem.Models;

namespace EmploymentSystem.Repositories
{
    public interface IVacancyRepository
    {
        Task<List<Vacancy>> GetAllAsync();
        Task<Vacancy?> GetByIdAsync(int id);
        Task AddAsync(Vacancy vacancy);
        Task UpdateAsync(Vacancy vacancy);
        Task DeleteAsync(int id);
        Task<List<Vacancy>> SearchAsync(string keyword);
        
    }

}
