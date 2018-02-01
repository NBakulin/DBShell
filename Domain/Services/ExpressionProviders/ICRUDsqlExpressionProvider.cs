using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Attribute;

namespace Domain.Services.ExpressionProviders
{
    // ReSharper disable once InconsistentNaming
    public interface ICRUDsqlExpressionProvider
    {
        string Insert(Table table, IDictionary<Attribute, string> values);

        string Delete(Table table, int id);

        string SelectAll(Table table, IEnumerable<Attribute> attributes);
    }
}