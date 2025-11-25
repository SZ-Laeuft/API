using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public EventsController(SZLDbContext context)
        {
            _context = context;
        }
        
        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
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
                .ToListAsync();

            return Success<IEnumerable<EventsDto>>(events);
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
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
                return Error<EventsDto>("Event not found", 404);

            return Success(evt);
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> PostEvent(EventsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<EventsDto>("Invalid event data", 422);

            // Optional: check if event with same name already exists
            if (await _context.Events.AnyAsync(e => e.Name == dto.Name))
                return Error<EventsDto>("Event with the same name already exists", 409);

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

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = evt.Eventid },
                new ApiResponse<EventsDto> { Success = true, Data = result }
            );
        }

        // PUT: api/events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid event data", 422);

            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
                return Error<object>("Event not found", 404);

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
                return Error<object>("Event not found", 404);

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
 
}
