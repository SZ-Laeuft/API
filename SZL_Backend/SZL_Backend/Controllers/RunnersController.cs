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
        [ProducesResponseType(typeof(IEnumerable<RunnersDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRunners()
        {
            try
            {
                var data = await _context.Runners
                    .Select(r => new RunnersDto
                    {
                        Runnerid = r.Runnerid,
                        Firstname = r.Firstname,
                        Lastname = r.Lastname
                    })
                    .OrderBy(r => r.Runnerid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/runners/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RunnersDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRunner(int id)
        {
            try
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

                return Ok(runner);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/runners
        [HttpPost]
        [ProducesResponseType(typeof(RunnersDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostRunner(RunnersCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Firstname) || string.IsNullOrWhiteSpace(dto.Lastname))
                return BadRequest("Firstname and Lastname are required");

            try
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
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/runners/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRunner(int id, RunnersCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Firstname) || string.IsNullOrWhiteSpace(dto.Lastname))
                return BadRequest("Firstname and Lastname are required");

            try
            {
                var runner = await _context.Runners.FindAsync(id);
                if (runner == null)
                    return NotFound();

                runner.Firstname = dto.Firstname;
                runner.Lastname = dto.Lastname;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/runners/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRunner(int id)
        {
            try
            {
                var runner = await _context.Runners.FindAsync(id);
                if (runner == null)
                    return NotFound();

                _context.Runners.Remove(runner);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
