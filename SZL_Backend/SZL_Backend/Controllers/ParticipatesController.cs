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
    public class ParticipatesController(SZLDbContext context) : ControllerBase
    {
        // GET: api/participates
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all participates",
            Description = "Retrieves a list of all participates in events."
        )]
        [ProducesResponseType(typeof(IEnumerable<ParticipatesDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetParticipates()
        {
            try
            {
                var data = await context.Participates
                    .Select(p => new ParticipatesDto
                    {
                        ParticipateId = p.Participateid,
                        TeamId = p.Teamid,
                        TagId = p.Tagid.ToString(),
                        RunnerId = p.Runnerid,
                        EventId = p.Eventid,
                        CategoryId = p.CategoryId
                    })
                    .OrderBy(p => p.ParticipateId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/participates/by-participateId/{id}
        [HttpGet("by-participateId/{id}")]
        [SwaggerOperation(
            Summary = "Get participate by ParticipateId",
            Description = "Retrieves a specific participate by its unique ParticipateId."
        )]
        [ProducesResponseType(typeof(ParticipatesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetParticipateById(int id)
        {
            try
            {
                var participate = await context.Participates
                    .Where(p => p.Participateid == id)
                    .Select(p => new ParticipatesDto
                    {
                        ParticipateId = p.Participateid,
                        TeamId = p.Teamid,
                        TagId = p.Tagid.ToString(),
                        RunnerId = p.Runnerid,
                        EventId = p.Eventid,
                        CategoryId = p.CategoryId
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

        // GET: api/participates/by-tagId/{id}
        [HttpGet("by-tagId/{id}")]
        [SwaggerOperation(
            Summary = "Get participates by TagId",
            Description = "Retrieves all participates associated with a specific TagId."
        )]
        [ProducesResponseType(typeof(IEnumerable<ParticipatesDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetParticipatesByTagId(string id)
        {
            if (!long.TryParse(id, out var tagId))
                return BadRequest("Invalid TagId");

            try
            {
                var participates = await context.Participates
                    .Where(p => p.Tagid == tagId)
                    .Select(p => new ParticipatesDto
                    {
                        ParticipateId = p.Participateid,
                        TeamId = p.Teamid,
                        TagId = p.Tagid.ToString(),
                        RunnerId = p.Runnerid,
                        EventId = p.Eventid,
                        CategoryId = p.CategoryId
                    })
                    .ToListAsync();

                if (!participates.Any())
                    return NotFound();

                return Ok(participates);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/participates
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new participate",
            Description = "Creates a new participate entry for a team in an event."
        )]
        [ProducesResponseType(typeof(ParticipatesDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostParticipate(ParticipatesCreateDto dto)
        {
            if (dto.TeamId <= 0 || dto.EventId <= 0)
                return BadRequest("TeamId and EventId must be valid");

            if (string.IsNullOrWhiteSpace(dto.TagId))
                return BadRequest("TagId is required");

            if (!long.TryParse(dto.TagId, out var tagId))
                return BadRequest("Invalid TagId");

            try
            {
                var participate = new Participate
                {
                    Teamid = dto.TeamId,
                    Tagid = tagId,
                    Runnerid = dto.RunnerId,
                    Eventid = dto.EventId
                };

                context.Participates.Add(participate);
                await context.SaveChangesAsync();

                var result = new ParticipatesDto
                {
                    ParticipateId = participate.Participateid,
                    TeamId = participate.Teamid,
                    TagId = participate.Tagid.ToString(),
                    RunnerId = participate.Runnerid,
                    EventId = participate.Eventid,
                    CategoryId = participate.CategoryId
                };

                return CreatedAtAction(nameof(GetParticipateById), new { id = participate.Participateid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/participates/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing participate",
            Description = "Updates the details of an existing participate entry."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutParticipate(int id, ParticipatesCreateDto dto)
        {
            if (dto.TeamId <= 0 || dto.EventId <= 0)
                return BadRequest("TeamId and EventId must be valid");

            try
            {
                var participate = await context.Participates.FindAsync(id);
                if (participate == null)
                    return NotFound();

                participate.Teamid = dto.TeamId;
                if (!string.IsNullOrWhiteSpace(dto.TagId))
                {
                    if (!long.TryParse(dto.TagId, out var tagId))
                        return BadRequest("Invalid TagId");
                    participate.Tagid = tagId;
                }
                participate.Runnerid = dto.RunnerId;
                participate.Eventid = dto.EventId;

                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/participates/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a participate",
            Description = "Deletes an existing participate entry identified by its ID."
        )]
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
