using CloudCart.Api.Domain.Entities;
using CloudCart.Api.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCart.Api.Persistence.Repositories
{
    public class DiscountEventRepository : RepositoryBase<DiscountEvent>, IDiscountEventRepository
    {
        public DiscountEventRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<List<DiscountEvent>> GetActiveDiscountsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _db.DiscountEvents
                .Where(d =>
                    d.StartDate <= currentDate &&
                    d.EndDate >= currentDate)
                .ToListAsync();
        }

        public async Task<List<DiscountEvent>> GetActiveDiscountsForProductAsync(int productId)
        {
            var currentDate = DateTime.UtcNow;
            return await _db.DiscountEvents
                .Where(de => 
                    de.StartDate <= currentDate && 
                    de.EndDate >= currentDate && 
                    de.Products.Any(p => 
                        p.Id == productId))
                .ToListAsync();
        }

        public async Task<DiscountEvent?> GetByIdWithProductsAsync(int id)
        {
            return await _db.DiscountEvents
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
