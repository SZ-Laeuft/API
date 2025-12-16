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
    public class BestTimeController(SZLDbContext context) : ControllerBase
    {
        // GET: api/besttime
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all best times",
            Description = "Retrieves a list of all best times available in the system."
        )]
        [ProducesResponseType(typeof(IEnumerable<BestTimeDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBestTimes()
        {
            try
            {
                var data = await context.Besttimes
                    .Select(b => new BestTimeDto
                    {
                        Besttimeid = b.Besttimeid,
                        Besttime1 = b.Besttime1,
                        ParticipateId = b.ParticipateId
                    })
                    .OrderBy(b => b.Besttimeid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/besttime/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get best time by ID",
            Description = "Retrieves a specific best time by its unique ID."
        )]
        [ProducesResponseType(typeof(BestTimeDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBestTime(int id)
        {
            try
            {
                var bestTime = await context.Besttimes
                    .Select(b => new BestTimeDto
                    {
                        Besttimeid = b.Besttimeid,
                        Besttime1 = b.Besttime1,
                        ParticipateId = b.ParticipateId
                    })
                    .FirstOrDefaultAsync(b => b.Besttimeid == id);

                if (bestTime == null)
                    return NotFound();

                return Ok(bestTime);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/besttime
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new best time",
            Description = "Creates a new best time entry."
        )]
        [ProducesResponseType(typeof(BestTimeDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostBestTime(BestTimeCreateDto dto)
        {
            try
            {
                var bestTime = new Besttime
                {
                    Besttime1 = dto.Besttime1,
                    ParticipateId = dto.ParticipateId
                };

                context.Besttimes.Add(bestTime);
                await context.SaveChangesAsync();

                var result = new BestTimeDto
                {
                    Besttimeid = bestTime.Besttimeid,
                    Besttime1 = bestTime.Besttime1,
                    ParticipateId = bestTime.ParticipateId
                };

                return CreatedAtAction(nameof(GetBestTime), new { id = bestTime.Besttimeid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/besttime/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing best time",
            Description = "Updates the details of an existing best time identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutBestTime(int id, BestTimeCreateDto dto)
        {
            try
            {
                var bestTime = await context.Besttimes.FindAsync(id);
                if (bestTime == null)
                    return NotFound();

                bestTime.Besttime1 = dto.Besttime1;
                bestTime.ParticipateId = dto.ParticipateId;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/besttime/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a best time",
            Description = "Deletes an existing best time identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBestTime(int id)
        {
            try
            {
                var bestTime = await context.Besttimes.FindAsync(id);
                if (bestTime == null)
                    return NotFound();

                context.Besttimes.Remove(bestTime);
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
