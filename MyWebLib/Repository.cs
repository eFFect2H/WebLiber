using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    // <summary>
    /// Базовый репозиторий на EF Core.
    /// </summary>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _set;
        private readonly IIncludeProvider<TEntity>? _includeProvider;

        public Repository(DbContext context, IIncludeProvider<TEntity>? includeProvider = null)
        {
            _context = context;
            _set = context.Set<TEntity>();
            _includeProvider = includeProvider;
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            var query = _set.AsQueryable();
            if (_includeProvider != null)
            {
                foreach (var include in _includeProvider.Includes())
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id")!.Equals(id));
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var query = _set.AsQueryable();
            if (_includeProvider != null)
            {
                foreach (var include in _includeProvider.Includes())
                    query = query.Include(include);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await _set.AsNoTracking().Where(predicate).ToListAsync();

        public async Task AddAsync(TEntity entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(TKey id) => await _set.FindAsync(id) != null;

        public IQueryable<TEntity> Query() => _set.AsQueryable();
    }
}
