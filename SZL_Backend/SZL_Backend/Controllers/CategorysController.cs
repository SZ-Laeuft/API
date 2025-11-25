using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Context;
using SZL_Backend.Dto;
using SZL_Backend.Entities;

namespace SZL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ApiControllerBase
    {
        private readonly SZLDbContext _context;

        public CategorysController(SZLDbContext context)
        {
            _context = context;
        }

        // GET: api/categorys
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategorysDto
                {
                    Categoryid = c.Categoryid,
                    Name = c.Name
                })
                .ToListAsync();

            return Success(categories);
        }

        // GET: api/categorys/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Select(c => new CategorysDto
                {
                    Categoryid = c.Categoryid,
                    Name = c.Name
                })
                .FirstOrDefaultAsync(c => c.Categoryid == id);

            if (category == null)
                return Error<CategorysDto>("Category not found", 404);

            return Success(category);
        }

        // POST: api/categorys
        [HttpPost]
        public async Task<IActionResult> PostCategory(CategorysCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<CategorysDto>("Invalid category data", 422);

            if (await _context.Categories.AnyAsync(c => c.Name == dto.Name))
                return Error<CategorysDto>("Category name already exists", 409);

            var category = new Category { Name = dto.Name };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategorysDto { Categoryid = category.Categoryid, Name = category.Name };

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.Categoryid },
                new ApiResponse<CategorysDto> { Success = true, Data = result }
            );
        }

        // PUT: api/categorys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategorysCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Error<object>("Invalid category data", 422);

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return Error<object>("Category not found", 404);

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
                return Error<object>("Category not found", 404);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
