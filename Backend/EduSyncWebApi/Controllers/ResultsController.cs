using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduSyncWebApi.Data;
using EduSyncWebApi.Models;

namespace EduSyncWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResultsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            return await _context.Results.ToListAsync();
        }

        // GET: api/Results/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Results/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(Guid id, Result result)
        {
            if (id != result.ResultId)
            {
                return BadRequest();
            }

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
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

        // POST: api/Results
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Result>> PostResult(Result result)
        {
            _context.Results.Add(result);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResultExists(result.ResultId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetResult", new { id = result.ResultId }, result);
        }

        // DELETE: api/Results/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultExists(Guid id)
        {
            return _context.Results.Any(e => e.ResultId == id);
        }
    }
}
