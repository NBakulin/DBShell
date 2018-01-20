namespace Domain.Services
{
    public interface IDeployable<in T> where T : Entities.Entity
    {
        bool IsDeployable(T entity);
    }
}