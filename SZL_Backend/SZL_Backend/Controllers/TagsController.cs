using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(SZLDbContext context) : ControllerBase
    {
        // GET: api/tags
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TagsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags()
        {
            try
            {
                var data = await context.Tags
                    .Select(t => new TagsDto
                    {
                        Tagid = t.Tagid,
                        Status = t.Status
                    })
                    .OrderBy(t => t.Tagid)
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/tags/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTag(int id)
        {
            try
            {
                var tag = await context.Tags
                    .Where(t => t.Tagid == id)
                    .Select(t => new TagsDto
                    {
                        Tagid = t.Tagid,
                        Status = t.Status
                    })
                    .FirstOrDefaultAsync();

                if (tag == null)
                    return NotFound();

                return Ok(tag);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/tags
        [HttpPost]
        [ProducesResponseType(typeof(TagsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostTag(TagsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status is required");

            try
            {
                var tag = new Tag
                {
                    Status = dto.Status
                };

                context.Tags.Add(tag);
                await context.SaveChangesAsync();

                var result = new TagsDto
                {
                    Tagid = tag.Tagid,
                    Status = tag.Status
                };

                return CreatedAtAction(nameof(GetTag), new { id = tag.Tagid }, result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/tags/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutTag(int id, TagsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status is required");

            try
            {
                var tag = await context.Tags.FindAsync(id);
                if (tag == null)
                    return NotFound();

                tag.Status = dto.Status;
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/tags/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                var tag = await context.Tags.FindAsync(id);
                if (tag == null)
                    return NotFound();

                context.Tags.Remove(tag);
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
