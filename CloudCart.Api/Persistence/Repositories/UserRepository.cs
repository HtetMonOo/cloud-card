using CloudCart.Api.Domain.Entities;
using CloudCart.Api.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
