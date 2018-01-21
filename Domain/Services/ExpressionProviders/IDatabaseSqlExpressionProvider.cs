using Domain.Entities;

namespace Domain.Services.ExpressionProviders
{
    public interface IDatabaseSqlExpressionProvider
    {
        string Create(Database database);

        string UpdateName(Database database);

        string Remove(Database database);
    }
}