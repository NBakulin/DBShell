using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Link;

namespace Domain.Services
{
    public interface IDeployService
    {
        bool IsDeployed(Database database);

        bool IsDeployPossible(Database database);

        void Deploy(Database database);

        void UpdateDeployed(Database database);

        void DropDeployedDatabase(Database database);

        void DropDeployedTable(Table table);

        void DropDeployedAttribute(Attribute attribute);

        void DropDeployedLink(Link link);
    }
}