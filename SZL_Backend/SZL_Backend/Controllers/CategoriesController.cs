using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;
using static SZL_Backend.Dto.CategoriesDto;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly SZLDbContext _context;

        public CategorysController(SZLDbContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoriesDto>), 200)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var data = await _context.Categories
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

       
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoriesDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)] 
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories
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
        
        [HttpPost]
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
                var exists = await _context.Categories.AnyAsync(c => c.Name == dto.Name);
                if (exists)
                    return Conflict("Category with this name already exists");

                var category = new Category { Name = dto.Name };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

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
        
        [HttpPut("{id}")]
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
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound(); 

                category.Name = dto.Name;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500); 
            }
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)] 
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound(); 

                _context.Categories.Remove(category);
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
