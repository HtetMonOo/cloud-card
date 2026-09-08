using CloudCart.Api.Domain.Entities;
using System.Threading.Tasks;

namespace CloudCart.Api.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
