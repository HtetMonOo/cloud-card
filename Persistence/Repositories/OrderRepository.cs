using CloudCart.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCart.Api.Persistence.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return await _db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersForUserAsync(int userId)
        {
            return await _db.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }
    }
}
