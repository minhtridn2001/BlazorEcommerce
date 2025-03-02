using System.Security.Claims;
using BlazorEcommerce.Data;
using BlazorEcommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/cartItems")]
    public class CartItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItems()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var cartItems = await _context.CartItem
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .Select(c => ToCartItemDTO(c))
                .ToListAsync();

            return Ok(cartItems);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> PutProductIntoCart(string productId, [FromQuery] int change = 1)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var existingCartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += change;
                if (existingCartItem.Quantity < 1)
                {
                    existingCartItem.Quantity = 0;
                }
            }
            else
            {
                var product = await _context.Product.FindAsync(productId);
                if (product == null) return BadRequest("Invalid ProductId");
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = change
                };
                _context.CartItem.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(string productId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static CartItemDTO ToCartItemDTO(CartItem cartItem)
        {
            return new CartItemDTO
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product.Price,
                ProductName = cartItem.Product.Name,
                ImageUrl = cartItem.Product.ImageUrl
            };
        }

    }
}
