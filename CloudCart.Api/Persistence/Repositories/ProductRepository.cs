using CloudCart.Api.Domain.Entities;
using CloudCart.Api.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCart.Api.Persistence.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _db.Products
                .AsNoTracking()
                .Include(p => p.OrderItems)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            return await _db.Products
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();     
        }
    }
}
