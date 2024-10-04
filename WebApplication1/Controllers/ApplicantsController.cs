using Castle.MicroKernel;
using EmploymentSystem.Models;
using EmploymentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmploymentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Applicant")]
    public class ApplicantsController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantsController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpGet("vacancies")]
        public async Task<IActionResult> SearchVacancies([FromQuery] string keyword)
        {
            try
            {
                var vacancies = await _applicantService.SearchVacancies(keyword);
                return Ok(vacancies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("vacancies/{vacancyId}/apply")]
        public async Task<IActionResult> ApplyForVacancy(int vacancyId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var applicantId = int.Parse(userIdClaim.Value);

            try
            {
                await _applicantService.ApplyForVacancy(vacancyId, applicantId);
                return Ok(new { Message = "Application submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("applied-vacancies")]
        public async Task<IActionResult> GetAppliedVacancies()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var applicantId = int.Parse(userIdClaim.Value);

            try
            {
                var appliedVacancies = await _applicantService.GetAppliedVacancies(applicantId);
                return Ok(appliedVacancies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
