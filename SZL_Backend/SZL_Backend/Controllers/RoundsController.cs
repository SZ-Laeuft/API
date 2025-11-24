using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.DTO;
using SZL_Backend.Entities;


namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public RoundsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/rounds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoundsDTO>>> GetRounds()
        {
            return await _context.Rounds
                .Select(r => new RoundsDTO
                {
                    Roundid = r.Roundid,
                    Participateid = r.Participateid,
                    Roundtimestamp = r.Roundtimestamp
                })
                .ToListAsync();
        }

        // GET: api/rounds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoundsDTO>> GetRound(int id)
        {
            var round = await _context.Rounds
                .Where(r => r.Roundid == id)
                .Select(r => new RoundsDTO
                {
                    Roundid = r.Roundid,
                    Participateid = r.Participateid,
                    Roundtimestamp = r.Roundtimestamp
                })
                .FirstOrDefaultAsync();

            if (round == null)
                return NotFound();

            return round;
        }

        // POST: api/rounds
        [HttpPost]
        public async Task<ActionResult<RoundsDTO>> PostRound(RoundsCreateDTO dto)
        {
            var round = new Round
            {
                Participateid = dto.Participateid,
                Roundtimestamp = dto.Roundtimestamp
            };

            _context.Rounds.Add(round);
            await _context.SaveChangesAsync();

            var result = new RoundsDTO
            {
                Roundid = round.Roundid,
                Participateid = round.Participateid,
                Roundtimestamp = round.Roundtimestamp
            };

            return CreatedAtAction(nameof(GetRound), new { id = round.Roundid }, result);
        }

        // PUT: api/rounds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRound(int id, RoundsCreateDTO dto)
        {
            var round = await _context.Rounds.FindAsync(id);
            if (round == null)
                return NotFound();

            round.Participateid = dto.Participateid;
            round.Roundtimestamp = dto.Roundtimestamp;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/rounds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRound(int id)
        {
            var round = await _context.Rounds.FindAsync(id);
            if (round == null)
                return NotFound();

            _context.Rounds.Remove(round);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
