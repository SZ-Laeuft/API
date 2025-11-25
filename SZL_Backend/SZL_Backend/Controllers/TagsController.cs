using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public TagsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _context.Tags
                .Select(t => new TagsDto
                {
                    Tagid = t.Tagid,
                    Status = t.Status
                })
                .ToListAsync();

            return Success(tags);
        }

        // GET: api/tags/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag = await _context.Tags
                .Where(t => t.Tagid == id)
                .Select(t => new TagsDto
                {
                    Tagid = t.Tagid,
                    Status = t.Status
                })
                .FirstOrDefaultAsync();

            if (tag == null)
                return Error<TagsDto>("Tag not found", 404);

            return Success(tag);
        }

        // POST: api/tags
        [HttpPost]
        public async Task<IActionResult> PostTag(TagsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<TagsDto>("Invalid tag data", 422);

            var tag = new Tag
            {
                Status = dto.Status
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            var result = new TagsDto
            {
                Tagid = tag.Tagid,
                Status = tag.Status
            };

            return CreatedAtAction(
                nameof(GetTag),
                new { id = tag.Tagid },
                new ApiResponse<TagsDto> { Success = true, Data = result }
            );
        }

        // PUT: api/tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagsCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid tag data", 422);

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return Error<object>("Tag not found", 404);

            tag.Status = dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return Error<object>("Tag not found", 404);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
