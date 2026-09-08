using CloudCart.Api.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudCart.Api.Persistence.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<List<Order>> GetOrdersForUserAsync(int userId);
    }
}
