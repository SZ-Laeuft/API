using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;


namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunnersController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public RunnersController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/runners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RunnersDto>>> GetRunners()
        {
            return await _context.Runners
                .Select(r => new RunnersDto
                {
                    Runnerid = r.Runnerid,
                    Firstname = r.Firstname,
                    Lastname = r.Lastname
                })
                .ToListAsync();
        }

        // GET: api/runners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RunnersDto>> GetRunner(int id)
        {
            var runner = await _context.Runners
                .Where(r => r.Runnerid == id)
                .Select(r => new RunnersDto
                {
                    Runnerid = r.Runnerid,
                    Firstname = r.Firstname,
                    Lastname = r.Lastname
                })
                .FirstOrDefaultAsync();

            if (runner == null)
                return NotFound();

            return runner;
        }

        // POST: api/runners
        [HttpPost]
        public async Task<ActionResult<RunnersDto>> PostRunner(RunnersCreateDto dto)
        {
            var runner = new Runner
            {
                Firstname = dto.Firstname,
                Lastname = dto.Lastname
            };

            _context.Runners.Add(runner);
            await _context.SaveChangesAsync();

            var result = new RunnersDto
            {
                Runnerid = runner.Runnerid,
                Firstname = runner.Firstname,
                Lastname = runner.Lastname
            };

            return CreatedAtAction(nameof(GetRunner), new { id = runner.Runnerid }, result);
        }

        // PUT: api/runners/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRunner(int id, RunnersCreateDto dto)
        {
            var runner = await _context.Runners.FindAsync(id);
            if (runner == null)
                return NotFound();

            runner.Firstname = dto.Firstname;
            runner.Lastname = dto.Lastname;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/runners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRunner(int id)
        {
            var runner = await _context.Runners.FindAsync(id);
            if (runner == null)
                return NotFound();

            _context.Runners.Remove(runner);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
