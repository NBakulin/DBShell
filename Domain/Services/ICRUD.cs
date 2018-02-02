using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Attribute;

namespace Domain.Services
{
    // ReSharper disable once InconsistentNaming
    public interface ICRUD
    {
        int Insert(Table table, IDictionary<Attribute, string> values);

        int Delete(Table table, int id);

        IEnumerable<IDictionary<Attribute, object>> SelectAll(Table table);
    }
}