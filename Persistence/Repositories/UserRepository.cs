using CloudCart.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CloudCart.Api.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
