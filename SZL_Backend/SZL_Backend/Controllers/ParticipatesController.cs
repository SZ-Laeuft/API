using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.DTO;
using SZL_Backend.Entities;


namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipatesController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public ParticipatesController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/participates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipatesDTO>>> GetParticipates()
        {
            return await _context.Participates
                .Select(p => new ParticipatesDTO
                {
                    Participateid = p.Participateid,
                    Teamid = p.Teamid,
                    Tagid = p.Tagid,
                    Runnerid = p.Runnerid,
                    Eventid = p.Eventid
                })
                .ToListAsync();
        }

        // GET: api/participates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipatesDTO>> GetParticipate(int id)
        {
            var participate = await _context.Participates
                .Where(p => p.Participateid == id)
                .Select(p => new ParticipatesDTO
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

            return participate;
        }

        // POST: api/participates
        [HttpPost]
        public async Task<ActionResult<ParticipatesDTO>> PostParticipate(ParticipatesCreateDTO dto)
        {
            var participate = new Participate
            {
                Teamid = dto.Teamid,
                Tagid = dto.Tagid,
                Runnerid = dto.Runnerid,
                Eventid = dto.Eventid
            };

            _context.Participates.Add(participate);
            await _context.SaveChangesAsync();

            var result = new ParticipatesDTO
            {
                Participateid = participate.Participateid,
                Teamid = participate.Teamid,
                Tagid = participate.Tagid,
                Runnerid = participate.Runnerid,
                Eventid = participate.Eventid
            };

            return CreatedAtAction(nameof(GetParticipate), new { id = participate.Participateid }, result);
        }

        // PUT: api/participates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipate(int id, ParticipatesCreateDTO dto)
        {
            var participate = await _context.Participates.FindAsync(id);
            if (participate == null)
                return NotFound();

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
                return NotFound();

            _context.Participates.Remove(participate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
