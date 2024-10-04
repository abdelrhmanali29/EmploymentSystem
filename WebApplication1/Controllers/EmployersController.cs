using Castle.MicroKernel;
using EmploymentSystem.DTOs;
using EmploymentSystem.Models;
using EmploymentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmploymentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public EmployersController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpPost("vacancies")]
        public async Task<IActionResult> CreateVacancy(VacancyDTO vacancyDto)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var employerId = int.Parse(userIdClaim.Value);

            try
            {
                await _employerService.CreateVacancy(vacancyDto, employerId);
                return Ok(new { Message = "Vacancy created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("vacancies/{vacancyId}")]
        public async Task<IActionResult> UpdateVacancy(int vacancyId, VacancyDTO vacancyDto)
        {
            try
            {
                await _employerService.UpdateVacancy(vacancyId, vacancyDto);
                return Ok(new { Message = "Vacancy updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("vacancies/{vacancyId}")]
        public async Task<IActionResult> DeleteVacancy(int vacancyId)
        {
            try
            {
                await _employerService.DeleteVacancy(vacancyId);
                return Ok(new { Message = "Vacancy deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("vacancies")]
        public async Task<IActionResult> GetEmployerVacancies()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var employerId = int.Parse(userIdClaim.Value);

            try
            {
                var vacancies = await _employerService.GetEmployerVacancies(employerId);
                return Ok(vacancies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("vacancies/{vacancyId}")]
        public async Task<IActionResult> GetVacancyById(int vacancyId)
        {
            try
            {
                var vacancy = await _employerService.GetVacancyById(vacancyId);
                return Ok(vacancy);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("vacancies/{vacancyId}/applications")]
        public async Task<IActionResult> GetVacancyApplications(int vacancyId)
        {
            try
            {
                var applications = await _employerService.GetVacancyApplications(vacancyId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("vacancies/archive")]
        public async Task<IActionResult> ArchiveExpiredVacancies()
        {
            try
            {
                await _employerService.ArchiveExpiredVacancies();
                return Ok(new { Message = "Expired vacancies archived successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
