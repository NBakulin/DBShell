using Domain.Entities;

namespace Domain.Services
{
    public interface IDeployService
    {
        bool IsDeployed(Database database);

        bool IsDeployable(Database database);

        void DeployDatabase(Database database);

        void DropDatabase(Database database);
    }
}