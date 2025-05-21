using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduSyncWebApi.Data;
using EduSyncWebApi.Models;
using EduSyncWebApi.DTO;

namespace EduSyncWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssessmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Assessments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assessment>>> GetAssessments()
        {
            return await _context.Assessments.ToListAsync();
        }

        // GET: api/Assessments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assessment>> GetAssessment(Guid id)
        {
            var assessment = await _context.Assessments.FindAsync(id);

            if (assessment == null)
            {
                return NotFound();
            }

            return assessment;
        }

        // GET: api/Assessments/GetCourse/{courseId}
        [HttpGet("GetCourse/{courseId}")]
        public async Task<ActionResult<IEnumerable<Assessment>>> GetAssessmentsByCourseId(Guid courseId)
        {
            var assessments = await _context.Assessments
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            return assessments;
        }

        // POST: api/Assessments
        [HttpPost]
        public async Task<ActionResult<Assessment>> PostAssessment([FromBody] AssessmentDTO assessment)
        {
            try
            {
                if (assessment.AssessmentId == Guid.Empty)
                {
                    assessment.AssessmentId = Guid.NewGuid();
                }

                Assessment originalAssessment = new Assessment()
                {
                    AssessmentId = assessment.AssessmentId,
                    CourseId = assessment.CourseId,
                    Title = assessment.Title,
                    Questions = assessment.Questions,
                    MaxScore = assessment.MaxScore
                };

                _context.Assessments.Add(originalAssessment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAssessment", new { id = originalAssessment.AssessmentId }, originalAssessment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Assessments/ByCourse/{courseId}/create
        [HttpPost("ByCourse/{courseId}/create")]
        [ProducesResponseType(typeof(Assessment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Assessment>> PostAssessmentByCourse([FromRoute] Guid courseId, [FromBody] AssessmentDTO assessment)
        {
            try
            {
                if (assessment.AssessmentId == Guid.Empty)
                {
                    assessment.AssessmentId = Guid.NewGuid();
                }

                Assessment originalAssessment = new Assessment()
                {
                    AssessmentId = assessment.AssessmentId,
                    CourseId = courseId,
                    Title = assessment.Title,
                    Questions = assessment.Questions,
                    MaxScore = assessment.MaxScore
                };

                _context.Assessments.Add(originalAssessment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAssessment", new { id = originalAssessment.AssessmentId }, originalAssessment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Assessments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessment(Guid id, AssessmentDTO assessment)
        {
            if (id != assessment.AssessmentId)
            {
                return BadRequest();
            }

            Assessment originalAssessment = new Assessment()
            {
                AssessmentId = assessment.AssessmentId,
                CourseId = assessment.CourseId,
                Title = assessment.Title,
                Questions = assessment.Questions,
                MaxScore = assessment.MaxScore
            };

            _context.Entry(originalAssessment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Assessments/{assessmentId}/Course/{courseId}
        [HttpPut("{assessmentId}/Course/{courseId}")]
        public async Task<IActionResult> PutAssessmentByCourse(Guid assessmentId, Guid courseId, AssessmentDTO assessment)
        {
            // only assert assessmentId from URL matches the DTO
            if (assessmentId != assessment.AssessmentId)
                return BadRequest("assessmentId in URL must match body.");

            // fetch *by* assessmentId alone
            var existing = await _context.Assessments
                .FirstOrDefaultAsync(a => a.AssessmentId == assessmentId);

            if (existing == null)
                return NotFound($"No assessment found with Id = {assessmentId}");

            // now you can move it to the new course
            existing.CourseId  = courseId;
            existing.Title     = assessment.Title;
            existing.Questions = assessment.Questions;
            existing.MaxScore  = assessment.MaxScore;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // handle concurrency if needed
                throw;
            }

            return NoContent();
        }



        // DELETE: api/Assessments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(Guid id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }

            _context.Assessments.Remove(assessment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssessmentExists(Guid id)
        {
            return _context.Assessments.Any(e => e.AssessmentId == id);
        }
    }
}
