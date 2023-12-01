using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core;
using ServiceManager.Core.Entities;
using ServiceManager.UI.Models.Base;

namespace ServiceManager.UI.Facades.Base
{
    public abstract class BaseFacade<TFull, TSave, TTable, TEntity>
        where TFull : BaseModel
        where TSave : BaseModel
        where TTable : BaseModel
        where TEntity : BaseEntity
    {
        protected readonly IMapper _mapper;
        protected ServicesContext _ctx;
        protected DbSet<TEntity> _dbSet => _ctx.Set<TEntity>();

        protected BaseFacade(ServicesContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public abstract IEnumerable<TTable> Table();

        public virtual TFull GetById(Guid Id)
        {
            var entity = _dbSet.Find(Id);
            return _mapper.Map<TFull>(entity);
        }

        public virtual Guid Save(TSave model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _dbSet.Update(entity);
            _ctx.SaveChanges();
            return entity.Id;
        }

        public virtual void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) 
                throw new NullReferenceException(nameof(entity));

            _dbSet.Remove(entity);
            _ctx.SaveChanges();
        }
    }
}
