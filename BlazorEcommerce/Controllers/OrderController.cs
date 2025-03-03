using BlazorEcommerce.Constants;
using System.Security.Claims;
using BlazorEcommerce.Data;
using BlazorEcommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Services;

namespace BlazorEcommerce.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderService _orderService;

        public OrderController(ApplicationDbContext dbContext, IOrderService orderService)
        {
            _dbContext = dbContext;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var createdOrder = await _orderService.CreateOrderAsync(userId, createOrderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var order = await _orderService.GetOrderAsync(id);  

            if (order == null)
                return NotFound();

            if (userId != order.UserId)
                return Unauthorized();

            return Ok(order);
        }

        private static OrderDTO ToOrderDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                ContactName = order.ContactName,
                Phone = order.Phone,
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                SubTotal = order.SubTotal,
                Tax = order.Tax,
                GrandTotal = order.GrandTotal,
                CreatedDate = order.CreatedDate,
                OrderStatus = order.OrderStatus.ToString(),
                PaymentStatus = order.PaymentStatus.ToString(),
                PaymentId = order.PaymentId,
                Items = order.Items.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}
