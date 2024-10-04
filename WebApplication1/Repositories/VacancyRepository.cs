using EmploymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmploymentSystem.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly AppDbContext _context;

        public VacancyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vacancy>> GetAllAsync()
        {
            return await _context.Vacancies.ToListAsync();
        }

        public async Task<Vacancy?> GetByIdAsync(int id)
        {
            return await _context.Vacancies
                         .Include(v => v.Applications)
                         .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Vacancy vacancy)
        {
            await _context.Vacancies.AddAsync(vacancy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vacancy vacancy)
        {
            _context.Vacancies.Update(vacancy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy != null)
            {
                _context.Vacancies.Remove(vacancy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Vacancy>> SearchAsync(string keyword)
        {
            return await _context.Vacancies
                .Where(v => v.Title.Contains(keyword) || v.Description.Contains(keyword))
                .ToListAsync();
        }
    }

}
