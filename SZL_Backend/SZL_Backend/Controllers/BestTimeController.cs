using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestTimeController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public BestTimeController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/besttime
        [HttpGet]
        public async Task<IActionResult> GetBestTimes()
        {
            try
            {
                var data = await _context.BestTimeViews
                    .Select(bt => new BestTimeViewDto
                    {
                        ParticipateId = bt.ParticipateId,
                        RoundId = bt.RoundId,
                        BestTime = bt.BestTime
                    })
                    .OrderBy(bt => bt.ParticipateId)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/besttime/1
        [HttpGet("{participateId}")]
        public async Task<IActionResult> GetBestTimeByParticipateId(int participateId)
        {
            try
            {
                var bestTime = await _context.BestTimeViews
                    .Where(bt => bt.ParticipateId == participateId)
                    .Select(bt => new BestTimeViewDto
                    {
                        ParticipateId = bt.ParticipateId,
                        RoundId = bt.RoundId,
                        BestTime = bt.BestTime
                    })
                    .FirstOrDefaultAsync();

                if (bestTime == null)
                    return NotFound();

                return Ok(bestTime);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}