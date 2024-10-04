using EmploymentSystem.DTOs;
using EmploymentSystem.Models;
using EmploymentSystem.Repositories;
using EmploymentSystem.RTOs;
using EmploymentSystem.Utilities;
using Microsoft.Extensions.Logging;

namespace EmploymentSystem.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<EmployerService> _logger;

        public EmployerService(IVacancyRepository vacancyRepository, IUserRepository userRepository, ILogger<EmployerService> logger)
        {
            _vacancyRepository = vacancyRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task CreateVacancy(VacancyDTO vacancyDto, int employerId)
        {
            _logger.LogInformation("Creating vacancy for employer {EmployerId}", employerId);

            var employer = await _userRepository.GetByIdAsync(employerId);
            if (employer == null)
            {
                _logger.LogWarning("Employer {EmployerId} not found.", employerId);
                throw new Exception("Employer not found");
            }

            var vacancy = new Vacancy
            {
                Title = vacancyDto.Title,
                Description = vacancyDto.Description,
                MaxApplications = vacancyDto.MaxApplications,
                ExpiryDate = vacancyDto.ExpiryDate,
                EmployerId = employerId,
                Employer = employer,
                IsActive = true,
                ApplicationsCount = 0
            };

            await _vacancyRepository.AddAsync(vacancy);

            _logger.LogInformation("Vacancy created successfully for employer {EmployerId}.", employerId);
        }

        public async Task UpdateVacancy(int vacancyId, VacancyDTO vacancyDto)
        {
            _logger.LogInformation("Updating vacancy {VacancyId}", vacancyId);
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                _logger.LogWarning("Vacancy {VacancyId} not found.", vacancyId);
                throw new Exception("Vacancy not found.");
            }

            vacancy.Title = vacancyDto.Title;
            vacancy.Description = vacancyDto.Description;
            vacancy.MaxApplications = vacancyDto.MaxApplications;
            vacancy.ExpiryDate = vacancyDto.ExpiryDate;

            await _vacancyRepository.UpdateAsync(vacancy);

            CacheManager.Remove($"VacancySearch_{vacancy.Title}");
            _logger.LogInformation("Cache cleared for updated vacancy: {VacancyTitle}", vacancy.Title);
            _logger.LogInformation("Vacancy {VacancyId} updated successfully.", vacancyId);
        }

        public async Task DeleteVacancy(int vacancyId)
        {
            _logger.LogInformation("Deleting vacancy {VacancyId}.", vacancyId);
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                _logger.LogWarning("Vacancy {VacancyId} not found.", vacancyId);
                throw new Exception("Vacancy not found.");
            }

            await _vacancyRepository.DeleteAsync(vacancyId);

            CacheManager.Remove($"VacancySearch_{vacancy.Title}");
            _logger.LogInformation("Cache cleared for deleted vacancy: {VacancyTitle}", vacancy.Title);
            _logger.LogInformation("Vacancy {VacancyId} deleted successfully.", vacancyId);
        }

        public async Task<List<Vacancy>> GetEmployerVacancies(int employerId)
        {
            _logger.LogInformation("Getting all vacancies for employer {EmployerId}.", employerId);
            return await _vacancyRepository.GetAllAsync();
        }

        public async Task<Vacancy> GetVacancyById(int vacancyId)
        {
            _logger.LogInformation("Getting vacancy by ID: {VacancyId}.", vacancyId);
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                _logger.LogWarning("Vacancy {VacancyId} not found.", vacancyId);
                throw new Exception("Vacancy not found.");
            }

            return vacancy;
        }

        public async Task<List<Application>> GetVacancyApplications(int vacancyId)
        {
            _logger.LogInformation("Getting applications for vacancy {VacancyId}.", vacancyId);
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            return vacancy == null
                ? throw new Exception("Vacancy not found.")
                : vacancy.Applications.ToList();
        }

        public async Task ArchiveExpiredVacancies()
        {
            _logger.LogInformation("Archiving expired vacancies.");
            var vacancies = await _vacancyRepository.GetAllAsync();
            foreach (var vacancy in vacancies)
            {
                if (vacancy.ExpiryDate < DateTime.Now && vacancy.IsActive)
                {
                    vacancy.IsActive = false;
                    await _vacancyRepository.UpdateAsync(vacancy);
                    _logger.LogInformation("Archived expired vacancy {VacancyId}.", vacancy.Id);
                }
            }
        }
    }
}
