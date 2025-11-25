using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
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
        [ProducesResponseType(typeof(IEnumerable<GiftsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetGifts()
        {
            try
            {
                var data = await _context.Gifts
                    .Select(g => new GiftsDto
                    {
                        Giftid = g.Giftid,
                        Name = g.Name,
                        Requirement = g.Requirement
                    })
                    .OrderBy(g => g.Giftid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/gifts/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GiftsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetGift(int id)
        {
            try
            {
                var gift = await _context.Gifts
                    .Where(g => g.Giftid == id)
                    .Select(g => new GiftsDto
                    {
                        Giftid = g.Giftid,
                        Name = g.Name,
                        Requirement = g.Requirement
                    })
                    .FirstOrDefaultAsync();

                if (gift == null)
                    return NotFound();

                return Ok(gift);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/gifts
        [HttpPost]
        [ProducesResponseType(typeof(GiftsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostGift(GiftsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            try
            {
                var gift = new Gift
                {
                    Name = dto.Name,
                    Requirement = dto.Requirement
                };

                _context.Gifts.Add(gift);
                await _context.SaveChangesAsync();

                var result = new GiftsDto
                {
                    Giftid = gift.Giftid,
                    Name = gift.Name,
                    Requirement = gift.Requirement
                };

                return CreatedAtAction(nameof(GetGift), new { id = gift.Giftid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/gifts/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutGift(int id, GiftsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            try
            {
                var gift = await _context.Gifts.FindAsync(id);
                if (gift == null)
                    return NotFound();

                gift.Name = dto.Name;
                gift.Requirement = dto.Requirement;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/gifts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteGift(int id)
        {
            try
            {
                var gift = await _context.Gifts.FindAsync(id);
                if (gift == null)
                    return NotFound();

                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
