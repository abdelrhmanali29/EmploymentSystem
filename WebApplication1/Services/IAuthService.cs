using EmploymentSystem.DTOs;

namespace EmploymentSystem.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO registerDto);
        Task<string> Login(LoginDTO loginDto);
    }
}