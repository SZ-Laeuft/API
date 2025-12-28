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
    public class TagsController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public TagsController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/tags
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all tags",
            Description = "Gets a list of all tags available"
        )]
        [ProducesResponseType(typeof(IEnumerable<TagsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags()
        {
            try
            {
                var data = await _context.Tags
                    .OrderBy(t => t.Tagid)
                    .Select(t => new TagsDto
                    {
                        TagId = t.Tagid.ToString(),
                        Status = t.Status
                    })
                    .ToListAsync();

                return Ok(data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/tags/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get tag by ID",
            Description = "Gets a specific tag by its unique ID."
        )]
        [ProducesResponseType(typeof(TagsDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTag(string id)
        {
            if (!long.TryParse(id, out var tagId))
                return BadRequest("Invalid ID");

            try
            {
                var tag = await _context.Tags
                    .Where(t => t.Tagid == tagId)
                    .Select(t => new TagsDto
                    {
                        TagId = t.Tagid.ToString(),
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
        [SwaggerOperation(
            Summary = "Create a new tag",
            Description = "Creates a new tag with the provided information."
        )]
        [ProducesResponseType(typeof(TagsDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostTag(TagsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status is required");

            if (!long.TryParse(dto.TagId, out var tagId))
                return BadRequest("Invalid TagId");

            try
            {
                var tag = new Tag
                {
                    Tagid = tagId,
                    Status = dto.Status
                };

                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                var result = new TagsDto
                {
                    TagId = tag.Tagid.ToString(),
                    Status = tag.Status
                };

                return CreatedAtAction(
                    nameof(GetTag),
                    new { id = result.TagId },
                    result
                );
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT: api/tags/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing tag",
            Description = "Updates the details of an existing tag identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutTag(string id, TagsCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status is required");

            if (!long.TryParse(id, out var tagId))
                return BadRequest("Invalid ID");

            try
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag == null)
                    return NotFound();

                tag.Status = dto.Status;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/tags/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a tag",
            Description = "Deletes an existing tag identified by its ID."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTag(string id)
        {
            if (!long.TryParse(id, out var tagId))
                return BadRequest("Invalid ID");

            try
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag == null)
                    return NotFound();

                _context.Tags.Remove(tag);
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
