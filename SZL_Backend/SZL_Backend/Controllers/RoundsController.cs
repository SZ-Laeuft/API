using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundsController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public RoundsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/rounds
        [HttpGet]
        public async Task<IActionResult> GetRounds()
        {
            var rounds = await _context.Rounds
                .Select(r => new RoundsDto
                {
                    Roundid = r.Roundid,
                    Participateid = r.Participateid,
                    Roundtimestamp = r.Roundtimestamp
                })
                .ToListAsync();

            return Success(rounds);
        }

        // GET: api/rounds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRound(int id)
        {
            var round = await _context.Rounds
                .Where(r => r.Roundid == id)
                .Select(r => new RoundsDto
                {
                    Roundid = r.Roundid,
                    Participateid = r.Participateid,
                    Roundtimestamp = r.Roundtimestamp
                })
                .FirstOrDefaultAsync();

            if (round == null)
                return Error<RoundsDto>("Round not found", 404);

            return Success(round);
        }

        // POST: api/rounds
        [HttpPost]
        public async Task<IActionResult> PostRound(RoundsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<RoundsDto>("Invalid round data", 422);

            var round = new Round
            {
                Participateid = dto.Participateid,
                Roundtimestamp = dto.Roundtimestamp
            };

            _context.Rounds.Add(round);
            await _context.SaveChangesAsync();

            var result = new RoundsDto
            {
                Roundid = round.Roundid,
                Participateid = round.Participateid,
                Roundtimestamp = round.Roundtimestamp
            };

            return CreatedAtAction(
                nameof(GetRound),
                new { id = round.Roundid },
                new ApiResponse<RoundsDto> { Success = true, Data = result }
            );
        }

        // PUT: api/rounds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRound(int id, RoundsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid round data", 422);

            var round = await _context.Rounds.FindAsync(id);
            if (round == null)
                return Error<object>("Round not found", 404);

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
                return Error<object>("Round not found", 404);

            _context.Rounds.Remove(round);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
