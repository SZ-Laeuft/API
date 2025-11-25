using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipatesController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public ParticipatesController(SZLDbContext context)
        {
            _context = context;
        }
        
        // GET: api/participates
        [HttpGet]
        public async Task<IActionResult> GetParticipates()
        {
            var participates = await _context.Participates
                .Select(p => new ParticipatesDto
                {
                    Participateid = p.Participateid,
                    Teamid = p.Teamid,
                    Tagid = p.Tagid,
                    Runnerid = p.Runnerid,
                    Eventid = p.Eventid
                })
                .ToListAsync();

            return Success(participates);
        }

        // GET: api/participates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipate(int id)
        {
            var participate = await _context.Participates
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
                return Error<ParticipatesDto>("Participate not found", 404);

            return Success(participate);
        }

        // POST: api/participates
        [HttpPost]
        public async Task<IActionResult> PostParticipate(ParticipatesCreateDto dto)
        {
            if (!ModelState.IsValid)
                return 
                    Error<ParticipatesDto>("Invalid participate data", 422);

            // Optional: check for duplicate participate for same runner in same event
            if (await _context.Participates.AnyAsync(p => p.Runnerid == dto.Runnerid && p.Eventid == dto.Eventid))
                return Error<ParticipatesDto>("This runner is already registered for the event", 409);

            var participate = new Participate
            {
                Teamid = dto.Teamid,
                Tagid = dto.Tagid,
                Runnerid = dto.Runnerid,
                Eventid = dto.Eventid
            };

            _context.Participates.Add(participate);
            await _context.SaveChangesAsync();

            var result = new ParticipatesDto
            {
                Participateid = participate.Participateid,
                Teamid = participate.Teamid,
                Tagid = participate.Tagid,
                Runnerid = participate.Runnerid,
                Eventid = participate.Eventid
            };

            return CreatedAtAction(
                nameof(GetParticipate),
                new { id = participate.Participateid },
                new ApiResponse<ParticipatesDto> { Success = true, Data = result }
            );
        }

        // PUT: api/participates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipate(int id, ParticipatesCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid participate data", 422);

            var participate = await _context.Participates.FindAsync(id);
            if (participate == null)
                return Error<object>("Participate not found", 404);

            participate.Teamid = dto.Teamid;
            participate.Tagid = dto.Tagid;
            participate.Runnerid = dto.Runnerid;
            participate.Eventid = dto.Eventid;

            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/participates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipate(int id)
        {
            var participate = await _context.Participates.FindAsync(id);
            if (participate == null)
                return Error<object>("Participate not found", 404);

            _context.Participates.Remove(participate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
