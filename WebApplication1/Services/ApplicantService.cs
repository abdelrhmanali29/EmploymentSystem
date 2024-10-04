using EmploymentSystem.DTOs;
using EmploymentSystem.Models;
using EmploymentSystem.Repositories;
using EmploymentSystem.RTOs;
using Microsoft.Extensions.Logging;

namespace EmploymentSystem.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ApplicantService> _logger;

        public ApplicantService(IVacancyRepository vacancyRepository, IApplicationRepository applicationRepository, IUserRepository userRepository, ILogger<ApplicantService> logger)
        {
            _vacancyRepository = vacancyRepository;
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<Vacancy>> SearchVacancies(string keyword)
        {
            _logger.LogInformation("Searching vacancies with keyword: {Keyword}", keyword);
            return await _vacancyRepository.SearchAsync(keyword);
        }

        public async Task ApplyForVacancy(int vacancyId, int applicantId)
        {
            _logger.LogInformation("Applicant {ApplicantId} is applying for vacancy {VacancyId}", applicantId, vacancyId);

            var canApply = await CanApplyForVacancy(applicantId);
            if (!canApply)
            {
                _logger.LogWarning("Applicant {ApplicantId} cannot apply for vacancy {VacancyId} more than once a day.", applicantId, vacancyId);
                throw new Exception("Cannot apply for more than one vacancy per day.");
            }

            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null || vacancy.ApplicationsCount >= vacancy.MaxApplications || !vacancy.IsActive)
            {
                _logger.LogWarning("Vacancy {VacancyId} is closed or full.", vacancyId);
                throw new Exception("Vacancy is closed or full.");
            }

            var applicant = await _userRepository.GetByIdAsync(applicantId);
            _logger.LogInformation("Applicant {ApplicantId} found for applying.", applicantId);

            var application = new Application
            {
                Vacancy = vacancy,
                Applicant = applicant,
                ApplicantId = applicantId,
                AppliedAt = DateTime.Now
            };

            await _applicationRepository.AddAsync(application);

            vacancy.ApplicationsCount++;
            await _vacancyRepository.UpdateAsync(vacancy);

            _logger.LogInformation("Applicant {ApplicantId} successfully applied for vacancy {VacancyId}.", applicantId, vacancyId);
        }

        public async Task<bool> CanApplyForVacancy(int applicantId)
        {
            _logger.LogInformation("Checking if applicant {ApplicantId} can apply.", applicantId);
            var applications = await _applicationRepository.GetByApplicantIdAsync(applicantId);
            return applications.All(a => a.AppliedAt < DateTime.Now.AddHours(-24));
        }

        public async Task<List<VacancyRTO>> GetAppliedVacancies(int applicantId)
        {
            _logger.LogInformation("Fetching applied vacancies for applicant {ApplicantId}.", applicantId);
            var applications = await _applicationRepository.GetByApplicantIdAsync(applicantId);

            return applications
                .Select(a => new VacancyRTO
                {
                    Id = a.Vacancy.Id,
                    Title = a.Vacancy.Title,
                    Description = a.Vacancy.Description,
                    ExpiryDate = a.Vacancy.ExpiryDate,
                    IsActive = a.Vacancy.IsActive
                })
                .ToList();
        }
    }
}
