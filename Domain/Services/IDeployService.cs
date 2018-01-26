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

        void RenameDeployedDatabase(Database database, string validNewDatabaseName);

        void DropDeployedDatabase(Database database);

        void RenameDeployedTable(Table table, string validNewTableName);

        void DropDeployedTable(Table table);

        void RenameDeployedAttribute(Attribute attribute, string validAttributeName);

        void DropDeployedAttribute(Attribute attribute);

        void DropDeployedLink(Link link);
    }
}