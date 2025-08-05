using Nest;
using Recipify.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Text;
using System.Threading.Tasks;
using Recipify.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Recipify.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey>
        : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
        where TEntity : class

    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context ;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            // return await _dbSet.Where(predicate);
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetByIdAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
