using BlazorEcommerce.Data;
using BlazorEcommerce.Services;
using BlazorEcommerce.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _orderService = new OrderService(_dbContext);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnEmpty_WhenNoOrdersExist()
        {
            // Act
            var result = await _orderService.GetAllOrdersAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateNewOrder()
        {
            // Arrange
            var userId = "user123";
            var createOrderDTO = new CreateOrderDTO
            {
                ContactName = "John Doe",
                Phone = "123456789",
                ShippingAddress = "123 Test Street",
                Notes = "Test order",
                Items = new List<CreateOrderItemDTO>
                {
                    new() { ProductId = "ProductId1", Quantity = 2 },
                    new() { ProductId = "ProductId2", Quantity = 1 }
                }
            };

            _dbContext.Product.Add(new Product { Id = "ProductId1", Name = "Product 1", Price = 10.0m });
            _dbContext.Product.Add(new Product { Id = "ProductId2", Name = "Product 2", Price = 20.0m });
            await _dbContext.SaveChangesAsync();

            // Act
            var order = await _orderService.CreateOrderAsync(userId, createOrderDTO);

            // Assert
            Assert.NotNull(order);
            Assert.Equal(userId, order.UserId);
            Assert.Equal(3, order.Items.Sum(i => i.Quantity));
            Assert.Equal(40m, order.SubTotal);
            Assert.Equal(4m, order.Tax);
            Assert.Equal(44m, order.GrandTotal);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldReturnCorrectOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = "user123",
                ContactName = "Jane Doe",
                Phone = "987654321",
                ShippingAddress = "456 Test Avenue",
                Notes = "Sample Order",
                SubTotal = 100m,
                Tax = 10m,
                GrandTotal = 110m,
                CreatedDate = DateTime.UtcNow,
                Items = new List<OrderItem>
                {
                    new() { ProductId = "ProductId1", ProductName = "Product A", Price = 50m, Quantity = 2 }
                }
            };

            _dbContext.Order.Add(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderService.GetOrderAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.UserId, result.UserId);
            Assert.Equal(2, result.Items.Sum(i => i.Quantity));
        }
    }
}
