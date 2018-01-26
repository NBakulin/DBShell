using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Repositories;

namespace Domain.Services.ExpressionProviders
{
    public sealed class LinkSqlExpressionProvider : ILinkSqlExpressionProvider
    {
        private readonly IRepository<Attribute> _attributeRepository;
        private readonly IRepository<Table> _tableRepository;

        public LinkSqlExpressionProvider(
            IRepository<Table> tableRepository,
            IRepository<Attribute> attributeRepository)
        {
            _tableRepository = tableRepository;
            _attributeRepository = attributeRepository;
        }

        public string Create(Link link)
        {
            var (master, slave) = (GetMasterTableName(link: link), GetSlaveTableName(link: link));

            return
                $"ALTER TABLE {slave} \n" +
                $"\tADD CONSTRAINT FK_{slave}_{master} FOREIGN KEY({master}Id) \n" +
                $"\tREFERENCES {master} (Id) \n" +
                $"\t\tON DELETE {(link.IsDeleteCascade ? "CASCADE" : "NO ACTION")} \n" +
                $"\t\tON UPDATE {(link.IsUpdateCascase ? "CASCADE" : "NO ACTION")}";
        }

        public string Remove(Link link)
        {
            var (master, slave) = (GetMasterTableName(link: link), GetSlaveTableName(link: link));

            return
                $"ALTER TABLE {slave} \n" +
                $"\tDROP FK_{slave}_{master}";
        }

        private string GetSlaveTableName(Link link)
        {
            ForeignKey foreignKey = _attributeRepository.All().Single(a => a.Id == link.SlaveAttributeId) as ForeignKey;

            Table table = _tableRepository.All().Single(t => t.Id == foreignKey.TableId);

            return table.Name;
        }

        private string GetMasterTableName(Link link)
        {
            PrimaryKey primaryKey = _attributeRepository.All().Single(a => a.Id == link.MasterAttributeId) as PrimaryKey;

            Table table = _tableRepository.All().Single(t => t.Id == primaryKey.TableId);

            return table.Name;
        }
    }
}