using CloudCart.Api.DTOs;

namespace CloudCart.Api.Services.Interfaces
{
    public interface IDiscountEventService
    {
        Task<DiscountEventDto> CreateDiscountEventAsync(CreateDiscountEventDto dto);
        Task<List<DiscountEventDto>> GetActiveDiscountsForProductAsync(int productId);
        Task<List<DiscountEventDto>> GetActiveDiscountsAsync();
        Task<DiscountEventDto> UpdateDiscountEventAsync(int id, UpdateDiscountEventDto dto);
    }
}
