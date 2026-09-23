using CloudCart.Api.DTOs;
using CloudCart.Api.Mapping;
using CloudCart.Api.Persistence.Repositories.Interfaces;
using CloudCart.Api.Services.Interfaces;

namespace CloudCart.Api.Services
{
    public class DiscountEventService: IDiscountEventService
    {
        private readonly IDiscountEventRepository _discountEventRepository;
        private readonly IProductRepository _productRepository;
        public DiscountEventService(IDiscountEventRepository discountEventRepository, IProductRepository productRepository)
        {
            _discountEventRepository = discountEventRepository;
            _productRepository = productRepository;
        }

        public async Task<DiscountEventDto> CreateDiscountEventAsync(CreateDiscountEventDto dto)
        {
            var products = await _productRepository.GetProductsByIdsAsync(dto.ProductIds);

            var discountEvent = dto.ToEntity();

            discountEvent.Products = products;

            await _discountEventRepository.AddAsync(discountEvent);
            await _discountEventRepository.SaveChangesAsync();

            var created = await _discountEventRepository.GetByIdAsync(discountEvent.Id);
            return created!.ToDto();
        }

        public async Task<List<DiscountEventDto>> GetActiveDiscountsForProductAsync(int productId)
        {
            var discountEvents = await _discountEventRepository.GetActiveDiscountsForProductAsync(productId);
            return discountEvents.Select(de => de.ToDto()).ToList();
        }

        public async Task<List<DiscountEventDto>> GetActiveDiscountsAsync()
        {
            var discountEvents = await _discountEventRepository.GetActiveDiscountsAsync();
            return discountEvents.Select(de => de.ToDto()).ToList();
        }
    }
}
