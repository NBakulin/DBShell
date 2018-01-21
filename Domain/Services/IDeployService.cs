using Domain.Entities;

namespace Domain.Services
{
    public interface IDeployService
    {
        bool IsDeployed(Database database);

        bool IsDeployPossible(Database database);

        void Deploy(Database database);

        void UpdateDeployed(Database database);

        void DropDeployed(Database database);
    }
}