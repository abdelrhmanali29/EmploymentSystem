using EmploymentSystem.Models;

namespace EmploymentSystem.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<bool> ExistsAsync(string username);
    }
}
