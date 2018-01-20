using Domain.Entities.Attribute;

namespace Domain.Services.ExpressionProviders
{
    public interface IAttributeSqlExpressionProvider
    {
        string Create(Attribute attribute);

        string Update(Attribute attribute);

        string Delele(Attribute attribute);

        string FullDefinition(Attribute attribute);
    }
}