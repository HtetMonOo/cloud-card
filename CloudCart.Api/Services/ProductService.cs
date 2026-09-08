using CloudCart.Api.DTOs;
using CloudCart.Api.Mapping;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCart.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var entity = dto.ToEntity();
            await _productRepository.AddAsync(entity);
            await _productRepository.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var list = await _productRepository.ListAsync();
            return list.Select(p => p.ToDto()).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _productRepository.GetByIdWithDetailsAsync(id);
            return p?.ToDto();
        }
    }
}
