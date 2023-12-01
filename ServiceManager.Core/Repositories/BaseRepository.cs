using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceManager.Core.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected ServicesContext _ctx { get; }
        protected DbSet<TEntity> _dbSet;

        public BaseRepository(ServicesContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<TEntity>();
        }

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstAsync(predicate);
        }
    }
}
