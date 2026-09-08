using CloudCart.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
    }
}
