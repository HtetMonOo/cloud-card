using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudCart.Api.Persistence.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _db;

        protected RepositoryBase(AppDbContext db)
        {
            _db = db;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public virtual async Task<List<T>> ListAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public virtual void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
