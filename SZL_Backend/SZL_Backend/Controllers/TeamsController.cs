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
    public class TeamsController(SZLDbContext context) : ControllerBase
    {
        // GET: api/teams
        [HttpGet]
        [SwaggerOperation (
            Summary = "Gets all teams" , 
            Description = "Gets a list of all teams available"
            )]
        [ProducesResponseType(typeof(IEnumerable<TeamsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTeams()
        {
            try
            {
                var data = await context.Teams
                    .Select(t => new TeamsDto
                    {
                        Teamid = t.Teamid,
                        Name = t.Name
                    })
                    .OrderBy(t => t.Teamid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/teams/5
        [HttpGet("{id}")]
        [SwaggerOperation (
            Summary = "Get team by ID", 
            Description = "Gets a specific team by its unique ID."
            )]
        [ProducesResponseType(typeof(TeamsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTeam(int id)
        {
            try
            {
                var team = await context.Teams
                    .Where(t => t.Teamid == id)
                    .Select(t => new TeamsDto
                    {
                        Teamid = t.Teamid,
                        Name = t.Name
                    })
                    .FirstOrDefaultAsync();

                if (team == null)
                    return NotFound();

                return Ok(team);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/teams
        [HttpPost]
        [SwaggerOperation (
            Summary = "Create a new team",
            Description = "Creates a new team with the provided information."
        )]
        [ProducesResponseType(typeof(TeamsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostTeam(TeamsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            try
            {
                var team = new Team
                {
                    Name = dto.Name
                };

                context.Teams.Add(team);
                await context.SaveChangesAsync();

                var result = new TeamsDto
                {
                    Teamid = team.Teamid,
                    Name = team.Name
                };

                return CreatedAtAction(nameof(GetTeam), new { id = team.Teamid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/teams/5
        [HttpPut("{id}")]
        [SwaggerOperation (
            Summary = "Update an existing team", 
            Description = "Updates the details of an existing team identified by its ID."
            )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutTeam(int id, TeamsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            try
            {
                var team = await context.Teams.FindAsync(id);
                if (team == null)
                    return NotFound();

                team.Name = dto.Name;
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/teams/5
        [HttpDelete("{id}")]
        [SwaggerOperation (
            Summary = "Delete a team",
            Description = "Deletes an existing team identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                var team = await context.Teams.FindAsync(id);
                if (team == null)
                    return NotFound();

                context.Teams.Remove(team);
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
