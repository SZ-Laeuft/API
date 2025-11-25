using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public GiftsController(SZLDbContext context)
        {
            _context = context;
        }
        
        // GET: api/gifts
        [HttpGet]
        public async Task<IActionResult> GetGifts()
        {
            var gifts = await _context.Gifts
                .Select(g => new GiftsDto
                {
                    Giftid = g.Giftid,
                    Name = g.Name,
                    Requirement = g.Requirement
                })
                .ToListAsync();

            return Success<IEnumerable<GiftsDto>>(gifts);
        }

        // GET: api/gifts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGift(int id)
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
                return Error<GiftsDto>("Gift not found", 404);

            return Success(gift);
        }

        // POST: api/gifts
        [HttpPost]
        public async Task<IActionResult> PostGift(GiftsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<GiftsDto>("Invalid gift data", 422);

            // Optional: check for duplicate gift name
            if (await _context.Gifts.AnyAsync(g => g.Name == dto.Name))
                return Error<GiftsDto>("Gift with the same name already exists", 409);

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

            return CreatedAtAction(
                nameof(GetGift),
                new { id = gift.Giftid },
                new ApiResponse<GiftsDto> { Success = true, Data = result }
            );
        }

        // PUT: api/gifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGift(int id, GiftsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid gift data", 422);

            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
                return Error<object>("Gift not found", 404);

            gift.Name = dto.Name;
            gift.Requirement = dto.Requirement;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // DELETE: api/gifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
                return Error<object>("Gift not found", 404);

            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
