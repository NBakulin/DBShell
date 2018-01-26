using Domain.Entities;

namespace Domain.Services.ExpressionProviders
{
    public interface IDeploySqlExpressionProvider
    {
        string SameNamedDatabaseCount(Database database);

        string DatabaseTablesNames(Database database);

        string SameNamedAttributes(Database database, Table table);
    }
}