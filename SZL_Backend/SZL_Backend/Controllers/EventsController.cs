using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.DTO;
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
        public async Task<ActionResult<IEnumerable<EventsDTO>>> GetEvents()
        {
            return await _context.Events
                .Select(e => new EventsDTO
                {
                    Eventid = e.Eventid,
                    Name = e.Name,
                    Place = e.Place,
                    Isactive = e.Isactive,
                    Starttime = e.Starttime,
                    Endtime = e.Endtime,
                    Categoryid = e.Categoryid
                })
                .ToListAsync();
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventsDTO>> GetEvent(int id)
        {
            var evt = await _context.Events
                .Where(e => e.Eventid == id)
                .Select(e => new EventsDTO
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

            return evt;
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<EventsDTO>> PostEvent(EventsCreateDTO dto)
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

            var result = new EventsDTO
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

        // PUT: api/events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventsCreateDTO dto)
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

        // DELETE: api/events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
                return NotFound();

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
