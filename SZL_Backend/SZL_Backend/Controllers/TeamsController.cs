using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;


namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public TeamsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamsDto>>> GetTeams()
        {
            return await _context.Teams
                .Select(t => new TeamsDto
                {
                    Teamid = t.Teamid,
                    Name = t.Name
                })
                .ToListAsync();
        }

        // GET: api/teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamsDto>> GetTeam(int id)
        {
            var team = await _context.Teams
                .Where(t => t.Teamid == id)
                .Select(t => new TeamsDto
                {
                    Teamid = t.Teamid,
                    Name = t.Name
                })
                .FirstOrDefaultAsync();

            if (team == null)
                return NotFound();

            return team;
        }

        // POST: api/teams
        [HttpPost]
        public async Task<ActionResult<TeamsDto>> PostTeam(TeamsCreateDto dto)
        {
            var team = new Team
            {
                Name = dto.Name
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var result = new TeamsDto
            {
                Teamid = team.Teamid,
                Name = team.Name
            };

            return CreatedAtAction(nameof(GetTeam), new { id = team.Teamid }, result);
        }

        // PUT: api/teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamsCreateDto dto)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();

            team.Name = dto.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
