using CloudCart.Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudCart.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
    }
}
