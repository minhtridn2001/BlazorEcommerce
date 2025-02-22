using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Data;
using BlazorEcommerce.Shared.DTO;
using BlazorEcommerce.Data.Pagination;
using Microsoft.Build.Framework;

namespace BlazorEcommerce.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDTO<ProductDTO>>> GetProducts(
            [FromQuery] int? categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Product.Where(x => !x.IsDeleted);
            if (categoryId.HasValue && categoryId != 0)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            var result = await query
                .Select(x => ToProductDTO(x))
                .ToPagedResultAsync(pageNumber, pageSize);
            return Ok(result);
        }

        private static ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };
        }

    }
}
