using BlazorEcommerce.Shared.DTO;

namespace BlazorEcommerce.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO createOrderDTO);

        Task<OrderDTO> GetOrderAsync(int orderId);
    }
}
