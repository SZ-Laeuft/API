using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using SZL_Backend.Context;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public TeamController(SZLDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create_Team")]
        [SwaggerOperation(Summary = "Create a new Team", Description = "This endpoint creates a new Team.")]
        [SwaggerResponse(200, "Team inserted successfully.", typeof(string))]
        [SwaggerResponse(500, "Internal Server Error.")]
        public async Task<IActionResult> CreateTeam([FromBody] Team team)
        {
            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return Ok($"Team '{team.Name}' inserted successfully with ID {team.Teamid}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}