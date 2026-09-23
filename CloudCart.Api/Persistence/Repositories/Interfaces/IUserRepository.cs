using CloudCart.Api.Domain.Entities;

namespace CloudCart.Api.Persistence.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
