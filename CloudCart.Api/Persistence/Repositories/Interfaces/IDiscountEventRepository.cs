using CloudCart.Api.Domain.Entities;

namespace CloudCart.Api.Persistence.Repositories.Interfaces
{
    public interface IDiscountEventRepository: IRepository<DiscountEvent>
    {
        Task<List<DiscountEvent>> GetActiveDiscountsAsync();

        Task<List<DiscountEvent>> GetActiveDiscountsForProductAsync(int productId);
    }
}
