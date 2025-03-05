using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorEcommerce.Controllers;
using BlazorEcommerce.Services;
using BlazorEcommerce.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BlazorEcommerce.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderController = new OrderController(_orderServiceMock.Object);
            SetUnauthenticatedUser();

        }

        [Fact]
        public async Task GetAllOrders_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var result = await _orderController.GetAllOrders();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOrders_WhenUserIsAuthenticated()
        {
            // Arrange
            SetAuthenticatedUser("user123");
            var orders = new PagedResultDTO<OrderDTO> { Items = new List<OrderDTO>(), TotalItems = 0 };
            _orderServiceMock.Setup(s => s.GetAllOrdersAsync("user123", 1, 10))
                             .ReturnsAsync(orders);

            // Act
            var result = await _orderController.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<PagedResultDTO<OrderDTO>>(okResult.Value);
        }

        [Fact]
        public async Task CreateOrder_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var createOrderDto = new CreateOrderDTO();

            // Act
            var result = await _orderController.CreateOrder(createOrderDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreated_WhenOrderIsCreated()
        {
            // Arrange
            SetAuthenticatedUser("user123");
            var createOrderDto = new CreateOrderDTO();
            var order = new OrderDTO { Id = 1, UserId = "user123" };
            _orderServiceMock.Setup(s => s.CreateOrderAsync("user123", createOrderDto))
                             .ReturnsAsync(order);

            // Act
            var result = await _orderController.CreateOrder(createOrderDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(1, ((OrderDTO) createdResult.Value).Id);
        }

        [Fact]
        public async Task GetOrderById_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var result = await _orderController.GetOrderById(1);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderById_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            SetAuthenticatedUser("user123");
            _orderServiceMock.Setup(s => s.GetOrderAsync(1)).ReturnsAsync((OrderDTO)null);

            // Act
            var result = await _orderController.GetOrderById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderById_ReturnsUnauthorized_WhenOrderDoesNotBelongToUser()
        {
            // Arrange
            SetAuthenticatedUser("user123");
            var order = new OrderDTO { Id = 1, UserId = "user456" };
            _orderServiceMock.Setup(s => s.GetOrderAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _orderController.GetOrderById(1);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOrder_WhenUserIsAuthorized()
        {
            // Arrange
            SetAuthenticatedUser("user123");
            var order = new OrderDTO { Id = 1, UserId = "user123" };
            _orderServiceMock.Setup(s => s.GetOrderAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _orderController.GetOrderById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(order.Id, ((OrderDTO)okResult.Value).Id);
        }

        private void SetAuthenticatedUser(string userId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _orderController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private void SetUnauthenticatedUser()
        {
            var user = new ClaimsPrincipal(); // No claims, or unauthenticated
            _orderController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
    }
}
