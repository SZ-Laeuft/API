using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;
using static SZL_Backend.Dto.CategorysDto;

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

        // GET: api/categorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategorysDto>>> GetCategories()
        {
            return await _context.Categories
                .Select(c => new CategorysDto
                {
                    Categoryid = c.Categoryid,
                    Name = c.Name
                })
                .ToListAsync();
        }

        // GET: api/categorys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategorysDto>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Select(c => new CategorysDto
                {
                    Categoryid = c.Categoryid,
                    Name = c.Name
                })
                .FirstOrDefaultAsync(c => c.Categoryid == id);

            if (category == null)
                return NotFound();

            return category;
        }

        // POST: api/categorys
        [HttpPost]
        public async Task<ActionResult<CategorysDto>> PostCategory(CategorysCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategorysDto
            {
                Categoryid = category.Categoryid,
                Name = category.Name
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Categoryid }, result);
        }


        // PUT: api/categorys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategorysCreateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/categorys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
