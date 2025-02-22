using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Data;
using BlazorEcommerce.Shared.DTO;

namespace BlazorEcommerce.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetProductCategories()
        {
            var categories = await _context.ProductCategory
                .Select(x => ToCategoryDTO(x))
                .ToListAsync();
            return Ok(categories);
        }

        private static CategoryDTO ToCategoryDTO(ProductCategory category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }

    }
}
