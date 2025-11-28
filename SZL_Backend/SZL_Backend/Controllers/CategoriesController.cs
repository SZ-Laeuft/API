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
    public class CategoriesController(SZLDbContext context) : ControllerBase
    {
        // GET: api/categories
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all categories",
            Description = "Retrieves a list of all categories available in the system."
        )]
        [ProducesResponseType(typeof(IEnumerable<CategoriesDto>), 200)] 
        [ProducesResponseType(500)] 
        
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var data = await context.Categories
                    .Select(c => new CategoriesDto
                    {
                        Categoryid = c.Categoryid,
                        Name = c.Name
                    })
                    .OrderBy(c => c.Name)
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
        [SwaggerOperation(
            Summary = "Get category by ID",
            Description = "Retrieves a specific category by its unique ID."
        )]
        [ProducesResponseType(typeof(CategoriesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)] 
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await context.Categories
                    .Select(c => new CategoriesDto
                    {
                        Categoryid = c.Categoryid,
                        Name = c.Name
                    })
                    .FirstOrDefaultAsync(c => c.Categoryid == id);

                if (category == null)
                    return NotFound();

                return Ok(category);
            }
            catch
            {
                return StatusCode(500); 
            }
        }
        
        // Post: api/categories
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new category",
            Description = "Creates a new category with the provided details."
        )]
        [ProducesResponseType(typeof(CategoriesDto), 201)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(409)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> PostCategory(CategorysCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name cannot be empty");

            try
            {
                var exists = await context.Categories.AnyAsync(c => c.Name == dto.Name);
                if (exists)
                    return Conflict("Category with this name already exists");

                var category = new Category { Name = dto.Name };
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                var result = new CategoriesDto
                {
                    Categoryid = category.Categoryid,
                    Name = category.Name
                };

                return CreatedAtAction(nameof(GetCategory), new { id = category.Categoryid }, result); 
            }
            catch
            {
                return StatusCode(500);
            }
        }
        
        // PUT: api/categories/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing category",
            Description = "Updates the details of an existing category identified by its ID."
        )]
        [ProducesResponseType(204)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(404)]
        [ProducesResponseType(500)] 
        public async Task<IActionResult> PutCategory(int id, CategorysCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name cannot be empty");

            try
            {
                var category = await context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound(); 

                category.Name = dto.Name;
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500); 
            }
        }
        
        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a category",
            Description = "Deletes an existing category identified by its ID."
        )]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)] 
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound(); 

                context.Categories.Remove(category);
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
