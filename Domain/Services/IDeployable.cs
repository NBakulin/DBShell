using Domain.Entities;

namespace Domain.Services
{
    public interface IDeployable<in T> where T : Entity
    {
        bool IsDeployable(T entity);
    }
}