using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipatesController(SZLDbContext context) : ControllerBase
    {
        // GET: api/participates
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ParticipatesDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetParticipates()
        {
            try
            {
                var data = await context.Participates
                    .Select(p => new ParticipatesDto
                    {
                        Participateid = p.Participateid,
                        Teamid = p.Teamid,
                        Tagid = p.Tagid,
                        Runnerid = p.Runnerid,
                        Eventid = p.Eventid
                    })
                    .OrderBy(p => p.Participateid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/participates/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ParticipatesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetParticipate(int id)
        {
            try
            {
                var participate = await context.Participates
                    .Where(p => p.Participateid == id)
                    .Select(p => new ParticipatesDto
                    {
                        Participateid = p.Participateid,
                        Teamid = p.Teamid,
                        Tagid = p.Tagid,
                        Runnerid = p.Runnerid,
                        Eventid = p.Eventid
                    })
                    .FirstOrDefaultAsync();

                if (participate == null)
                    return NotFound();

                return Ok(participate);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/participates
        [HttpPost]
        [ProducesResponseType(typeof(ParticipatesDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostParticipate(ParticipatesCreateDto dto)
        {
            if (dto.Teamid <= 0 || dto.Eventid <= 0)
                return BadRequest("TeamId and EventId must be valid");

            try
            {
                var participate = new Participate
                {
                    Teamid = dto.Teamid,
                    Tagid = dto.Tagid,
                    Runnerid = dto.Runnerid,
                    Eventid = dto.Eventid
                };

                context.Participates.Add(participate);
                await context.SaveChangesAsync();

                var result = new ParticipatesDto
                {
                    Participateid = participate.Participateid,
                    Teamid = participate.Teamid,
                    Tagid = participate.Tagid,
                    Runnerid = participate.Runnerid,
                    Eventid = participate.Eventid
                };

                return CreatedAtAction(nameof(GetParticipate), new { id = participate.Participateid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/participates/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutParticipate(int id, ParticipatesCreateDto dto)
        {
            if (dto.Teamid <= 0 || dto.Eventid <= 0)
                return BadRequest("TeamId and EventId must be valid");

            try
            {
                var participate = await context.Participates.FindAsync(id);
                if (participate == null)
                    return NotFound();

                participate.Teamid = dto.Teamid;
                participate.Tagid = dto.Tagid;
                participate.Runnerid = dto.Runnerid;
                participate.Eventid = dto.Eventid;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/participates/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteParticipate(int id)
        {
            try
            {
                var participate = await context.Participates.FindAsync(id);
                if (participate == null)
                    return NotFound();

                context.Participates.Remove(participate);
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
