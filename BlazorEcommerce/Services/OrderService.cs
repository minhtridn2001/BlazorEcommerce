using BlazorEcommerce.Constants;
using BlazorEcommerce.Data;
using BlazorEcommerce.Data.Pagination;
using BlazorEcommerce.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResultDTO<OrderDTO>> GetAllOrdersAsync(string? userId, int pageNumber = 1, int pageSize = 10)
        {
            var orders = await _dbContext.Order
                .Where(o => userId == null || o.UserId == userId)
                .Include(o => o.Items)
                .Select(x => ToOrderDTO(x))
                .ToPagedResultAsync(pageNumber, pageSize);
            return orders;
        }

        public async Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO createOrderDTO)
        {
            var productIds = createOrderDTO.Items.Select(i => i.ProductId).ToList();
            var products = await _dbContext.Product
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p);

            if (products.Count != productIds.Count)
                throw new ArgumentException("One or more products are invalid.");

            decimal subTotal = createOrderDTO.Items.Sum(i => products[i.ProductId].Price * i.Quantity);
            decimal tax = subTotal * 0.1m;
            decimal grandTotal = subTotal + tax;

            var order = new Order
            {
                UserId = userId,
                ContactName = createOrderDTO.ContactName,
                Phone = createOrderDTO.Phone,
                ShippingAddress = createOrderDTO.ShippingAddress,
                Notes = createOrderDTO.Notes,
                SubTotal = subTotal,
                Tax = tax,
                GrandTotal = grandTotal,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                Items = createOrderDTO.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    ProductName = products[i.ProductId].Name,
                    Price = products[i.ProductId].Price
                }).ToList()
            };

            _dbContext.Order.Add(order);
            await _dbContext.SaveChangesAsync();
            return ToOrderDTO(order);
        }

        public async Task<OrderDTO> GetOrderAsync(int orderId)
        {
            var order = await _dbContext.Order
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return ToOrderDTO(order);
        }

        private static OrderDTO ToOrderDTO(Order order)
        {
            if (order == null)
                return null;

            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
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
