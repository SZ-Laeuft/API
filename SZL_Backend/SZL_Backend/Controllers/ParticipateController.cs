using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipateController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public ParticipateController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/Participate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participate>>> GetParticipates()
        {
            return await _context.Participates.ToListAsync();
        }

        // GET: api/Participate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Participate>> GetParticipate(int id)
        {
            var participate = await _context.Participates.FindAsync(id);

            if (participate == null)
            {
                return NotFound();
            }

            return participate;
        }

        // PUT: api/Participate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipate(int id, Participate participate)
        {
            if (id != participate.Participateid)
            {
                return BadRequest();
            }

            _context.Entry(participate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Participate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Participate>> PostParticipate(Participate participate)
        {
            _context.Participates.Add(participate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipate", new { id = participate.Participateid }, participate);
        }

        // DELETE: api/Participate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipate(int id)
        {
            var participate = await _context.Participates.FindAsync(id);
            if (participate == null)
            {
                return NotFound();
            }

            _context.Participates.Remove(participate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipateExists(int id)
        {
            return _context.Participates.Any(e => e.Participateid == id);
        }
    }
}
