using Domain.Entities.Link;

namespace Domain.Services.ExpressionProviders
{
    public interface ILinkSqlExpressionProvider
    {
        string Create(Link link);

        string Remove(Link link);
    }
}