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
    public class TagsController(SZLDbContext context) : ControllerBase
    {
        // GET: api/tags
        [HttpGet]
        [SwaggerOperation (
            Summary = "Get all tags" , 
            Description = "Gets a list of all tags available"
            )]
        [ProducesResponseType(typeof(IEnumerable<TagsDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags()
        {
            try
            {
                var data = await context.Tags
                    .Select(t => new TagsDto
                    {
                        TagId = t.Tagid,
                        Status = t.Status
                    })
                    .OrderBy(t => t.TagId)
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
        [SwaggerOperation (
            Summary = "Get tag by ID", 
            Description = "Gets a specific tag by its unique ID."
            )]
        [ProducesResponseType(typeof(TagsDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTag(int id)
        {
            try
            {
                var tag = await context.Tags
                    .Select(t => new TagsDto
                    {
                        TagId = t.Tagid,
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
        [SwaggerOperation (
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

            try
            {
                var tag = new Tag
                {
                    Tagid = (int)dto.TagId!,
                    Status = dto.Status
                };

                context.Tags.Add(tag);
                await context.SaveChangesAsync();

                var result = new TagsDto
                {
                    TagId = tag.Tagid,
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
        [SwaggerOperation (
            Summary = "Update an existing tag", 
            Description = "Updates the details of an existing tag identified by its ID."
            )]
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
        [SwaggerOperation (
            Summary = "Delete a tag", 
            Description = "Deletes an existing tag identified by its ID."
            )]
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
