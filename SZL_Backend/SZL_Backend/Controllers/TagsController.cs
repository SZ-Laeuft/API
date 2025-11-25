using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public TagsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagsDto>>> GetTags()
        {
            return await _context.Tags
                .Select(t => new TagsDto
                {
                    Tagid = t.Tagid,
                    Status = t.Status
                })
                .ToListAsync();
        }

        // GET: api/tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TagsDto>> GetTag(int id)
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
                return NotFound();

            return tag;
        }

        // POST: api/tags
        [HttpPost]
        public async Task<ActionResult<TagsDto>> PostTag(TagsCreateDto dto)
        {
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

            return CreatedAtAction(nameof(GetTag), new { id = tag.Tagid }, result);
        }

        // PUT: api/tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagsCreateDto dto)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

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
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
