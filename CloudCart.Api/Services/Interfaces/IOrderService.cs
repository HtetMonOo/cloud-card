using CloudCart.Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudCart.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersForUserAsync(int userId);
    }
}
