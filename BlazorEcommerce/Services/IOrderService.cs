using BlazorEcommerce.Shared.DTO;

namespace BlazorEcommerce.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO createOrderDTO);

        Task<OrderDTO> GetOrderAsync(int orderId);

        /// <summary>
        /// Retrieves a paginated list of orders.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are to be retrieved. If null, retrieves all orders - this is helpful for admins.</param>
        /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of orders per page. Defaults to 10.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a PagedResultDTO of OrderDTO.</returns>
        Task<PagedResultDTO<OrderDTO>> GetAllOrdersAsync(string? userId, int pageNumber = 1, int pageSize = 10);
    }
}
