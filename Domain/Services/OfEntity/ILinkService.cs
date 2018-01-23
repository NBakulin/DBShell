using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;

namespace Domain.Services.OfEntity
{
    public interface ILinkService : IDeployable<Link>
    {
        void Add(
            Table masterTable,
            Table slaveTable,
            bool isCascadeDelete,
            bool isCascadeUpdate);

        void Remove(Link link);

        IEnumerable<Link> GetDatabaseLinks(Database database);

        Link GetLink(Table masterTable, Table slaveTable);

        PrimaryKey GetPrimaryKey(Table table);

        IEnumerable<ForeignKey> GetForeignKeys(Table table);

        ForeignKey GetForeignKey(Table masterTable, Table slaveTable);
    }
}