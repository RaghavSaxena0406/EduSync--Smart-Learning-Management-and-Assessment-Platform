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
using Microsoft.AspNetCore.Authorization;

namespace EduSyncWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //[Authorize(Roles = "Instructor")]
        //public async Task<IActionResult> PutCourse(Guid id, CourseDTO course)
        //{
        //    var userIdClaim = User.FindFirst("UserId")?.Value;
        //    if (!Guid.TryParse(userIdClaim, out var instructorId))
        //        return Unauthorized("Missing or invalid instructor token.");

        //    var existingCourse = await _context.Courses.FindAsync(id);
        //    if (existingCourse == null)
        //        return NotFound();

        //    if (existingCourse.InstructorId != instructorId)
        //        return Forbid("You can only edit your own courses.");

        //    existingCourse.Title = course.Title;
        //    existingCourse.Description = course.Description;
        //    existingCourse.MediaUrl = course.MediaUrl;

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        // PUT: api/Courses/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> PutCourse(Guid id, CourseDTO course)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!Guid.TryParse(userIdClaim, out var instructorId))
                return Unauthorized("Missing or invalid instructor token.");

            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null)
                return NotFound();

            if (existingCourse.InstructorId != instructorId)
                return Forbid("You can only edit your own courses.");

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.MediaUrl = course.MediaUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<Course>> PostCourse(CourseDTO course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ Securely extract InstructorId from the JWT token
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid instructorId))
            {
                return Unauthorized("Invalid or missing user ID claim.");
            }

            // Create Course entity
            Course newCourse = new Course()
            {
                CourseId = course.CourseId != Guid.Empty ? course.CourseId : Guid.NewGuid(),
                Title = course.Title,
                Description = course.Description,
                InstructorId = instructorId, // ✅ Use token-derived value
                MediaUrl = course.MediaUrl
            };

            _context.Courses.Add(newCourse);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(newCourse.CourseId))
                {
                    return Conflict("A course with the same ID already exists.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourse", new { id = newCourse.CourseId }, newCourse);
        }


        // DELETE: api/Courses/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCourse(Guid id)
        //{
        //    var course = await _context.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Courses.Remove(course);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!Guid.TryParse(userIdClaim, out var instructorId))
                return Unauthorized("Missing or invalid instructor token.");

            if (course.InstructorId != instructorId)
                return Forbid("You can only delete your own courses.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(Guid id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
