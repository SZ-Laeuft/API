using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(
            Summary = "Get all rounds",
            Description = "Retrieves a list of all rounds recorded."
        )]
        [ProducesResponseType(typeof(IEnumerable<RoundsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRounds()
        {
            try
            {
                var data = await context.Rounds
                    .Select(r => new RoundsDto
                    {
                        RoundId = r.Roundid,
                        ParticipateId = r.Participateid,
                        RoundTimeStamp = r.Roundtimestamp
                    })
                    .OrderBy(r => r.RoundId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/rounds/5
        [HttpGet("by-roundId/{roundId}")]
        [SwaggerOperation(
            Summary = "Get round by  round ID",
            Description = "Retrieves a specific round by its unique ID."
        )]
        [ProducesResponseType(typeof(RoundsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRoundRoundId(int roundId)
        {
            try
            {
                var round = await context.Rounds
                    .Where(r => r.Roundid == roundId)
                    .Select(r => new RoundsDto
                    {
                        RoundId = r.Roundid,
                        ParticipateId = r.Participateid,
                        RoundTimeStamp = r.Roundtimestamp
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
        
        // GET: api/rounds/5
        [HttpGet("by-participateId/{participateId}")]
        [SwaggerOperation(
            Summary = "Get round by participate ID",
            Description = "Retrieves a specific round by its unique ID."
        )]
        [ProducesResponseType(typeof(RoundsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRoundParticipateId(int participateId)
        {
            try
            {
                var round = await context.Rounds
                    .Where(r => r.Participateid == participateId)
                    .Select(r => new RoundsDto
                    {
                        RoundId = r.Roundid,
                        ParticipateId = r.Participateid,
                        RoundTimeStamp = r.Roundtimestamp
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
        
        // GET: api/rounds/5
        [HttpGet("get-round-count/{participateId}")]
        [SwaggerOperation(
            Summary = "Get amount of rounds",
            Description = "Retrieves amount of rounds by its unique ID."
        )]
        [ProducesResponseType(typeof(RoundsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRoundsParticipateId(int participateId)
        {
            try
            {
                var rounds = await context.Rounds
                    .Where(r => r.Participateid == participateId)
                    .CountAsync();
                return Ok(rounds);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/rounds
        [HttpPost]
        [SwaggerOperation (
            Summary = "Create a new round",
            Description = "Creates a new round record with the provided details."
        )]
        [ProducesResponseType(typeof(RoundsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostRound(RoundsCreateDto dto)
        {
            if (dto.ParticipateId <= 0)
                return BadRequest("ParticipateId must be greater than zero");

            try
            {
                var round = new Round
                {
                    Participateid = dto.ParticipateId,
                    Roundtimestamp = dto.RoundTimeStamp
                };

                context.Rounds.Add(round);
                await context.SaveChangesAsync();

                var result = new RoundsDto
                {
                    RoundId = round.Roundid,
                    ParticipateId = round.Participateid,
                    RoundTimeStamp = round.Roundtimestamp
                };

                return CreatedAtAction(nameof(GetRoundRoundId), new { roundId = round.Roundid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/rounds/5
        [HttpPut("{id}")]
        [SwaggerOperation (
            Summary = "Update an existing round",
            Description = "Updates the details of an existing round identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRound(int id, RoundsCreateDto dto)
        {
            if (dto.ParticipateId <= 0)
                return BadRequest("ParticipateId must be greater than zero");

            try
            {
                var round = await context.Rounds.FindAsync(id);
                if (round == null)
                    return NotFound();

                round.Participateid = dto.ParticipateId;
                round.Roundtimestamp = dto.RoundTimeStamp;

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
        [SwaggerOperation (
            Summary = "Delete a round",
            Description = "Deletes an existing round identified by its ID."
        )]
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
