using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core;
using ServiceManager.Core.Entities;
using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Facades.Base
{
    public abstract class BaseFacadeAsync<TFull, TSave, TTable, TEntity>
        where TFull : class
        where TSave : class
        where TTable : class
        where TEntity : BaseEntity
    {
        protected readonly IMapper _mapper;
        protected ServicesContext _ctx;
        protected DbSet<TEntity> _dbSet => _ctx.Set<TEntity>();

        protected BaseFacadeAsync(ServicesContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public abstract IQueryable<TTable> Table();

        public virtual async Task<TFull> GetById(Guid Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            return _mapper.Map<TFull>(entity);
        }

        public virtual async Task<Guid> Save(TSave model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _dbSet.Update(entity);
            await _ctx.SaveChangesAsync();
            if (model is BaseModel baseModel) baseModel.Id = entity.Id;
            return entity.Id;
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new NullReferenceException(nameof(entity));

            _dbSet.Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
