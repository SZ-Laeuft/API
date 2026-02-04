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
    public class EventsController(SZLDbContext context) : ControllerBase
    {
        // GET: api/events
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all events",
            Description = "Gets all events"
        )]
        [ProducesResponseType(typeof(IEnumerable<EventsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var data = await context.Events
                    .OrderBy(e => e.Eventid)
                    .Select(e => new EventsDto
                    {
                        EventId = e.Eventid,
                        Name = e.Name,
                        Place = e.Place,
                        IsActive = e.Isactive,
                        StartTime = e.Starttime,
                        EndTime = e.Endtime
                    })
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get event by ID",
            Description = "Gets a specific event by its ID"
        )]
        [ProducesResponseType(typeof(EventsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var evt = await context.Events
                    .Where(e => e.Eventid == id)
                    .Select(e => new EventsDto
                    {
                        EventId = e.Eventid,
                        Name = e.Name,
                        Place = e.Place,
                        IsActive = e.Isactive,
                        StartTime = e.Starttime,
                        EndTime = e.Endtime
                    })
                    .FirstOrDefaultAsync();

                if (evt == null)
                    return NotFound();

                return Ok(evt);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/events
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new event",
            Description = "Creates a new event. Only one event can be active at a time."
        )]
        [ProducesResponseType(typeof(EventsDto), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostEvent(EventsCreateDto dto)
        {
            var status = dto.IsActive.ToLowerInvariant();

            try
            {
               
                if (status == "active")
                {
                    var activeEvents = await context.Events
                        .Where(e => e.Isactive == "active")
                        .ToListAsync();

                    foreach (var e in activeEvents)
                        e.Isactive = "inactive";
                }

                var evt = new Event
                {
                    Name = dto.Name,
                    Place = dto.Place,
                    Isactive = status,
                    Starttime = dto.StartTime,
                    Endtime = dto.EndTime
                };

                context.Events.Add(evt);
                await context.SaveChangesAsync();

                var result = new EventsDto
                {
                    EventId = evt.Eventid,
                    Name = evt.Name,
                    Place = evt.Place,
                    IsActive = evt.Isactive,
                    StartTime = evt.Starttime,
                    EndTime = evt.Endtime
                };

                return CreatedAtAction(nameof(GetEvent), new { id = evt.Eventid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/events/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an event",
            Description = "Updates an event. Setting it to active will deactivate all others."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutEvent(int id, EventsCreateDto dto)
        {
            var status = dto.IsActive.ToLowerInvariant();

            try
            {
                var evt = await context.Events.FindAsync(id);
                if (evt == null)
                    return NotFound();
                
                if (status == "active")
                {
                    var activeEvents = await context.Events
                        .Where(e => e.Isactive == "active" && e.Eventid != id)
                        .ToListAsync();

                    foreach (var e in activeEvents)
                        e.Isactive = "inactive";
                }

                evt.Name = dto.Name;
                evt.Place = dto.Place;
                evt.Isactive = status;
                evt.Starttime = dto.StartTime;
                evt.Endtime = dto.EndTime;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/events/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete an event",
            Description = "Deletes an event by its ID"
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var evt = await context.Events.FindAsync(id);
                if (evt == null)
                    return NotFound();

                context.Events.Remove(evt);
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
