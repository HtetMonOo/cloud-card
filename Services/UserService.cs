using CloudCart.Api.DTOs;
using CloudCart.Api.Mapping;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCart.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var entity = dto.ToEntity();
            await _userRepository.AddAsync(entity);
            await _userRepository.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var list = await _userRepository.ListAsync();
            return list.Select(u => u.ToDto()).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            return u?.ToDto();
        }
    }
}
