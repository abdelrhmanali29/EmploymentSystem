using EmploymentSystem.DTOs;
using EmploymentSystem.Models;
using EmploymentSystem.Repositories;
using EmploymentSystem.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmploymentSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> Register(RegisterDTO registerDto)
        {
            _logger.LogInformation("Registering user {Username}", registerDto.Username);
            if (await _userRepository.ExistsAsync(registerDto.Username))
            {
                _logger.LogWarning("User {Username} already exists.", registerDto.Username);
                throw new Exception("User already exists.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = SecurityHelper.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                CreatedAt = DateTime.Now
            };

            await _userRepository.AddAsync(user);

            _logger.LogInformation("User {Username} registered successfully.", registerDto.Username);
            return GenerateJwtToken(user);
        }

        public async Task<string> Login(LoginDTO loginDto)
        {
            _logger.LogInformation("User {Username} is trying to log in.", loginDto.Username);
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || !SecurityHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for user {Username}.", loginDto.Username);
                throw new Exception("Invalid username or password.");
            }

            _logger.LogInformation("User {Username} logged in successfully.", loginDto.Username);
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            _logger.LogInformation("JWT token generated for user {UserId}.", user.Id);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
