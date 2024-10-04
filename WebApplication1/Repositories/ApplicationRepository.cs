using EmploymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmploymentSystem.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Application>> GetByVacancyIdAsync(int vacancyId)
        {
            return await _context.Applications
                .Where(a => a.VacancyId == vacancyId)
                .ToListAsync();
        }

        public async Task<Application> GetByApplicantIdAndVacancyIdAsync(int applicantId, int vacancyId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.ApplicantId == applicantId && a.VacancyId == vacancyId);
        }

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Application>> GetByApplicantIdAsync(int applicantId)
        {
            return await _context.Applications
                .Where(a => a.ApplicantId == applicantId)
                .Include(a => a.Vacancy)
                .ToListAsync();
        }
    }



}
