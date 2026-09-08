using CloudCart.Api.Domain.Entities;
using System.Threading.Tasks;

namespace CloudCart.Api.Persistence.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByIdWithDetailsAsync(int id);
    }
}
