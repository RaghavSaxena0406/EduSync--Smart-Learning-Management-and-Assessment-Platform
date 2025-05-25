using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduSyncWebApi.Data;
using EduSyncWebApi.Models;
using EduSyncWebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EduSyncWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssessmentResultsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssessmentResultsController(AppDbContext context)
        {
            _context = context;
        }

       // GET: api/AssessmentResults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentResultDTO>>> GetAssessmentResults()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing UserId claim.");

            if (string.IsNullOrEmpty(roleClaim))
                return Unauthorized("Missing role claim.");

            IQueryable<AssessmentResult> query;

            if (roleClaim.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                query = _context.AssessmentResults
                    .Include(r => r.Assessment)
                    .Where(r => r.StudentId == userId);
            }
            else if (roleClaim.Equals("Instructor", StringComparison.OrdinalIgnoreCase))
            {
                query = _context.AssessmentResults
                    .Include(r => r.Assessment)
                    .ThenInclude(a => a.Course)
                    .Where(r => r.Assessment.Course.InstructorId == userId);
            }
            else
            {
                return Forbid("Only students or instructors can access results.");
            }

            var results = await query
                .Select(result => new AssessmentResultDTO(result))
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/AssessmentResults/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentResultDTO>> GetAssessmentResult(Guid id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing UserId claim.");

            if (string.IsNullOrEmpty(roleClaim))
                return Unauthorized("Missing role claim.");

            var result = await _context.AssessmentResults
                .Include(r => r.Assessment)
                .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null)
                return NotFound();

            if (roleClaim.Equals("Student", StringComparison.OrdinalIgnoreCase) && result.StudentId != userId)
                return Forbid("Students can only access their own results.");

            if (roleClaim.Equals("Instructor", StringComparison.OrdinalIgnoreCase) &&
                result.Assessment?.Course?.InstructorId != userId)
                return Forbid("Instructors can only access results from their own courses.");

            return new AssessmentResultDTO(result);
        }


        // GET: api/AssessmentResults/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<AssessmentResultDTO>>> GetResultsByStudentId(Guid studentId)
        {
            return await _context.AssessmentResults
                .Include(r => r.Assessment)
                .Where(r => r.StudentId == studentId)
                .Select(result => new AssessmentResultDTO(result))
                .ToListAsync();
        }

        // GET: api/AssessmentResults/assessment/{assessmentId}
        [HttpGet("assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<AssessmentResultDTO>>> GetResultsByAssessmentId(Guid assessmentId)
        {
            return await _context.AssessmentResults
                .Where(r => r.AssessmentId == assessmentId)
                .Select(result => new AssessmentResultDTO(result))
                .ToListAsync();
        }

        // POST: api/AssessmentResults
        [HttpPost]
        public async Task<ActionResult<AssessmentResultDTO>> PostAssessmentResult(CreateAssessmentResultDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            var userExists = await _context.UserModels.AnyAsync(u => u.UserId == dto.StudentId);
            if (!userExists)
            {
                return BadRequest($"Student with ID {dto.StudentId} does not exist.");
            }

            var result = new AssessmentResult
            {
                ResultId = Guid.NewGuid(),
                AssessmentId = dto.AssessmentId,
                StudentId = dto.StudentId,
                Score = dto.Score,
                MaxScore = dto.MaxScore,
                SubmissionDate = DateTime.UtcNow,
                Answers = dto.Answers
            };

            _context.AssessmentResults.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssessmentResult), new { id = result.ResultId }, new AssessmentResultDTO(result));
        }


        // PUT: api/AssessmentResults/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessmentResult(Guid id, UpdateAssessmentResultDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _context.AssessmentResults.FindAsync(id);
            if (result == null)
                return NotFound();

            result.Score = dto.Score;
            result.MaxScore = dto.MaxScore;
            result.Answers = dto.Answers;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/AssessmentResults/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessmentResult(Guid id)
        {
            var result = await _context.AssessmentResults.FindAsync(id);
            if (result == null)
                return NotFound();

            _context.AssessmentResults.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
