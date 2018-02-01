using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute;

namespace Domain.Services.ExpressionProviders
{
    // ReSharper disable once InconsistentNaming
    public class CRUDsqlExpressionProvider : ICRUDsqlExpressionProvider
    {
        public string Insert(Table table, IDictionary<Attribute, string> values)
        {
            return
                $"INSERT INTO {table.Name} " +
                "(" +
                values
                    .Keys
                    .Aggregate(
                        seed: string.Empty,
                        func: (sum, key) => $"{sum}, {key.Name}")
                    .Remove(startIndex: 0, count: 1) +
                ")\n" +
                "VALUES " +
                "(" +
                values
                    .Keys
                    .Aggregate(
                        seed: string.Empty,
                        func: (sum, key) => $"{sum}, \'{values[key: key]}\'")
                    .Remove(startIndex: 0, count: 1) +
                ")";
        }

        public string Delete(Table table, int id)
        {
            return
                $"DELETE FROM {table.Name} \n" +
                $"WHERE Id = {id}";
        }

        public string SelectAll(Table table, IEnumerable<Attribute> attributes)
        {
            return
                "SELECT " +
                attributes
                    .Aggregate(
                        seed: string.Empty,
                        func: (sum, attr) => $"{sum}, {attr.Name}")
                    .Remove(startIndex: 0, count: 1) +
                " " +
                $"FROM {table.Name}";
        }
    }
}