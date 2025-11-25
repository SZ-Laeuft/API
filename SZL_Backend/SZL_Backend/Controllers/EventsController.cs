using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public EventsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var data = await _context.Events
                    .Select(e => new EventsDto
                    {
                        Eventid = e.Eventid,
                        Name = e.Name,
                        Place = e.Place,
                        Isactive = e.Isactive,
                        Starttime = e.Starttime,
                        Endtime = e.Endtime,
                        Categoryid = e.Categoryid
                    })
                    .OrderBy(e => e.Eventid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var evt = await _context.Events
                    .Where(e => e.Eventid == id)
                    .Select(e => new EventsDto
                    {
                        Eventid = e.Eventid,
                        Name = e.Name,
                        Place = e.Place,
                        Isactive = e.Isactive,
                        Starttime = e.Starttime,
                        Endtime = e.Endtime,
                        Categoryid = e.Categoryid
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
        [ProducesResponseType(typeof(EventsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostEvent(EventsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Place))
                return BadRequest("Name and Place are required");

            try
            {
                var evt = new Event
                {
                    Name = dto.Name,
                    Place = dto.Place,
                    Isactive = dto.Isactive,
                    Starttime = dto.Starttime,
                    Endtime = dto.Endtime,
                    Categoryid = dto.Categoryid
                };

                _context.Events.Add(evt);
                await _context.SaveChangesAsync();

                var result = new EventsDto
                {
                    Eventid = evt.Eventid,
                    Name = evt.Name,
                    Place = evt.Place,
                    Isactive = evt.Isactive,
                    Starttime = evt.Starttime,
                    Endtime = evt.Endtime,
                    Categoryid = evt.Categoryid
                };

                return CreatedAtAction(nameof(GetEvent), new { id = evt.Eventid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/events/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutEvent(int id, EventsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Place))
                return BadRequest("Name and Place are required");

            try
            {
                var evt = await _context.Events.FindAsync(id);
                if (evt == null)
                    return NotFound();

                evt.Name = dto.Name;
                evt.Place = dto.Place;
                evt.Isactive = dto.Isactive;
                evt.Starttime = dto.Starttime;
                evt.Endtime = dto.Endtime;
                evt.Categoryid = dto.Categoryid;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/events/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var evt = await _context.Events.FindAsync(id);
                if (evt == null)
                    return NotFound();

                _context.Events.Remove(evt);
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
