using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundsController(SZLDbContext context) : ControllerBase
    {
        // GET: api/rounds
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoundsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRounds()
        {
            try
            {
                var data = await context.Rounds
                    .Select(r => new RoundsDto
                    {
                        Roundid = r.Roundid,
                        Participateid = r.Participateid,
                        Roundtimestamp = r.Roundtimestamp
                    })
                    .OrderBy(r => r.Roundid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/rounds/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoundsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRound(int id)
        {
            try
            {
                var round = await context.Rounds
                    .Where(r => r.Roundid == id)
                    .Select(r => new RoundsDto
                    {
                        Roundid = r.Roundid,
                        Participateid = r.Participateid,
                        Roundtimestamp = r.Roundtimestamp
                    })
                    .FirstOrDefaultAsync();

                if (round == null)
                    return NotFound();

                return Ok(round);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/rounds
        [HttpPost]
        [ProducesResponseType(typeof(RoundsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostRound(RoundsCreateDto dto)
        {
            if (dto.Participateid <= 0)
                return BadRequest("ParticipateId must be greater than zero");

            try
            {
                var round = new Round
                {
                    Participateid = dto.Participateid,
                    Roundtimestamp = dto.Roundtimestamp
                };

                context.Rounds.Add(round);
                await context.SaveChangesAsync();

                var result = new RoundsDto
                {
                    Roundid = round.Roundid,
                    Participateid = round.Participateid,
                    Roundtimestamp = round.Roundtimestamp
                };

                return CreatedAtAction(nameof(GetRound), new { id = round.Roundid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/rounds/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRound(int id, RoundsCreateDto dto)
        {
            if (dto.Participateid <= 0)
                return BadRequest("ParticipateId must be greater than zero");

            try
            {
                var round = await context.Rounds.FindAsync(id);
                if (round == null)
                    return NotFound();

                round.Participateid = dto.Participateid;
                round.Roundtimestamp = dto.Roundtimestamp;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/rounds/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRound(int id)
        {
            try
            {
                var round = await context.Rounds.FindAsync(id);
                if (round == null)
                    return NotFound();

                context.Rounds.Remove(round);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
