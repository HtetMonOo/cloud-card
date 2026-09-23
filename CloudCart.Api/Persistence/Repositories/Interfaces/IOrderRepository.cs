using CloudCart.Api.Domain.Entities;

namespace CloudCart.Api.Persistence.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<List<Order>> GetOrdersForUserAsync(int userId);
    }
}
