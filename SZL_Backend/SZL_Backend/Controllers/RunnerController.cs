using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunnerController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public RunnerController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/Runner
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Runner>>> GetRunners()
        {
            return await _context.Runners.ToListAsync();
        }

        // GET: api/Runner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Runner>> GetRunner(int id)
        {
            var runner = await _context.Runners.FindAsync(id);

            if (runner == null)
            {
                return NotFound();
            }

            return runner;
        }

        // PUT: api/Runner/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRunner(int id, Runner runner)
        {
            if (id != runner.Runnerid)
            {
                return BadRequest();
            }

            _context.Entry(runner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RunnerExists(id))
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

        // POST: api/Runner
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Runner>> PostRunner(Runner runner)
        {
            _context.Runners.Add(runner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRunner", new { id = runner.Runnerid }, runner);
        }

        // DELETE: api/Runner/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRunner(int id)
        {
            var runner = await _context.Runners.FindAsync(id);
            if (runner == null)
            {
                return NotFound();
            }

            _context.Runners.Remove(runner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RunnerExists(int id)
        {
            return _context.Runners.Any(e => e.Runnerid == id);
        }
    }
}
