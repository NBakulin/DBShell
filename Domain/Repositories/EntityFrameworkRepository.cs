using System.Data.Entity;
using System.Linq;
using Domain.Entities;

namespace Domain.Repositories
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _set;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        void IRepository<TEntity>.Add(TEntity entity)
        {
            _set.Add(entity: entity);
            _context.SaveChanges();
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            _context.Entry(entity: entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        void IRepository<TEntity>.Remove(TEntity entity)
        {
            _set.Remove(entity: entity);
            _context.SaveChanges();
        }

        IQueryable<TEntity> IRepository<TEntity>.All()
        {
            return _set;
        }
    }
}