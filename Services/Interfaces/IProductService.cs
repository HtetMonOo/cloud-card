using CloudCart.Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudCart.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
    }
}
