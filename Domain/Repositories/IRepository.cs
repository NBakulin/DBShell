using System.Linq;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        IQueryable<TEntity> All();
    }
}