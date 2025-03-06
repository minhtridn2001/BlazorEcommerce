using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Data;
using BlazorEcommerce.Shared.DTO;
using BlazorEcommerce.Data.Pagination;
using Microsoft.Build.Framework;
using System.Security.Claims;

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
            [FromQuery] int? categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? orderBy = null)
        {
            var query = _context.Product.Where(x => !x.IsDeleted);
            if (categoryId.HasValue && categoryId != 0)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            var result = await query
                .ToPagedResultAsync(pageNumber, pageSize, orderBy);

            var pagedResultDTO = new PagedResultDTO<ProductDTO>(
                result.Items.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    CategoryId = x.CategoryId
                }).ToList(),
                result.TotalItems,
                result.PageNumber,
                result.PageSize
            );
            return Ok(pagedResultDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(string id)
        {
            var product = await _context.Product
                .Where(p => p.Id == id)
                .Select(p => ToProductDTO(p))
                .FirstOrDefaultAsync(); 

            if (product == null)
                return NotFound();

            return Ok(product);
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
