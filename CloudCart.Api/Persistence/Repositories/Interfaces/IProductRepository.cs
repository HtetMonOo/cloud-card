using CloudCart.Api.Domain.Entities;

namespace CloudCart.Api.Persistence.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<List<Product>> GetProductsByIdsAsync(List<int> ids);
    }
}
