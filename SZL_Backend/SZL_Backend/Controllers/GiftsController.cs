using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.DTO;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public GiftsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/gifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiftsDTO>>> GetGifts()
        {
            return await _context.Gifts
                .Select(g => new GiftsDTO
                {
                    Giftid = g.Giftid,
                    Name = g.Name,
                    Requirement = g.Requirement
                })
                .ToListAsync();
        }

        // GET: api/gifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GiftsDTO>> GetGift(int id)
        {
            var gift = await _context.Gifts
                .Where(g => g.Giftid == id)
                .Select(g => new GiftsDTO
                {
                    Giftid = g.Giftid,
                    Name = g.Name,
                    Requirement = g.Requirement
                })
                .FirstOrDefaultAsync();

            if (gift == null)
                return NotFound();

            return gift;
        }

        // POST: api/gifts
        [HttpPost]
        public async Task<ActionResult<GiftsDTO>> PostGift(GiftsCreateDTO dto)
        {
            var gift = new Gift
            {
                Name = dto.Name,
                Requirement = dto.Requirement
            };

            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();

            var result = new GiftsDTO
            {
                Giftid = gift.Giftid,
                Name = gift.Name,
                Requirement = gift.Requirement
            };

            return CreatedAtAction(nameof(GetGift), new { id = gift.Giftid }, result);
        }

        // PUT: api/gifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGift(int id, GiftsCreateDTO dto)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
                return NotFound();

            gift.Name = dto.Name;
            gift.Requirement = dto.Requirement;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/gifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
                return NotFound();

            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
