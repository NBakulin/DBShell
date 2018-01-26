using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Link;

namespace Domain.Services.OfEntity
{
    public interface ITableService : IDeployable<Table>
    {
        void Add(Database database, string tableName);

        void Rename(Table table, string name);

        IEnumerable<Table> GetDatabaseTables(Database database);

        IEnumerable<Link> GetDatabaseLinks(Database database);

        IEnumerable<Attribute> GetTableAttributes(Table table);

        Table GetTableByName(Database database, string name);

        Table GetTableById(int tableId);

        void RemoveTable(Table table);

        void RemoveLink(Link tink);

        bool HasPrimaryKey(Table table);

        void OffModified(Table table);
    }
}