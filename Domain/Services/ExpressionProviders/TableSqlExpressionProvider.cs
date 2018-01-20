using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Repositories;

namespace Domain.Services.ExpressionProviders
{
    public sealed class TableSqlExpressionProvider : ITableSqlExpressionProvider
    {
        private readonly IAttributeSqlExpressionProvider _attributeExpressionProvider;
        private readonly IRepository<Attribute> _attributeRepository;

        public TableSqlExpressionProvider(
            IAttributeSqlExpressionProvider attributeExpressionProvider,
            IRepository<Attribute> attributeRepository)
        {
            _attributeExpressionProvider = attributeExpressionProvider;
            _attributeRepository = attributeRepository;
        }

        public string CreateEmpty(Table table)
        {
            return
                $"CREATE TABLE {table.Name} \n" +
                "( \n" +
                _attributeExpressionProvider
                    .FullDefinition(
                        _attributeRepository
                            .All()
                            .SingleOrDefault(a => a.TableId == table.Id && a is PrimaryKey)) +
                ")";
        }

        public string CreateFull(Table table)
        {
            return
                $"CREATE TABLE {table.Name} \n" +
                "(\n" +
                _attributeRepository
                    .All()
                    .Where(a => a.TableId == table.Id)
                    .ToList()
                    .Aggregate(string.Empty, (total, attribute) =>
                        $"{total}, \n\t{_attributeExpressionProvider.FullDefinition(attribute)}")
                    .Remove(0, 3) +
                "\n)";
        }

        public string Remove(Table table)
        {
            return
                $"DROP TABLE {table.Name}";
        }
    }
}