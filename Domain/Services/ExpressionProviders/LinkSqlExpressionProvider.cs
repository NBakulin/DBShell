using System;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Link;
using Domain.Repositories;

namespace Domain.Services.ExpressionProviders
{
    public sealed class LinkSqlExpressionProvider : ILinkSqlExpressionProvider
    {
        private readonly IRepository<Table> _tableRepository;

        public LinkSqlExpressionProvider(
            IRepository<Table> tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public string Create(Link link)
        {
            var (master, slave) = (GetMasterTableName(link), GetSlaveTableName(link));

            return
                $"ALTER TABLE {slave} \n" +
                $"\tADD CONSTRAINT FK_{slave}_{master} FOREIGN KEY({master}Id) \n" +
                $"\tREFERENCES {master} (Id) \n" +
                $"\t\tON DELETE {(link.IsDeleteCascade ? "CASCADE" : "NO ACTION")} \n" +
                $"\t\tON UPDATE {(link.IsUpdateCascase ? "CASCADE" : "NO ACTION")}";
        }

        public string Remove(Link link)
        {
            var (master, slave) = (GetMasterTableName(link), GetSlaveTableName(link));

            return
                $"ALTER TABLE {slave} \n" +
                $"\tDROP FK_{slave}_{master}";
        }

        private string GetSlaveTableName(Link link)
        {
            return
                _tableRepository
                    .All()
                    .SingleOrDefault(t => t.Id == link.SlaveAttribute.TableId)
                    ?.Name
                ?? throw new ArgumentNullException(link.ToString(),
                    "Failure when loading \"slave\" table of the link.");
        }

        private string GetMasterTableName(Link link)
        {
            return
                _tableRepository
                    .All()
                    .SingleOrDefault(t => t.Id == link.MasterAttribute.TableId)
                    ?.Name
                ?? throw new ArgumentNullException(link.ToString(),
                    "Failure when loading \"master\" table of the link.");
        }
    }
}