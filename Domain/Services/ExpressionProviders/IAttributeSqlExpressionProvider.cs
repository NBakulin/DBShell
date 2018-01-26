using Domain.Entities.Attribute;

namespace Domain.Services.ExpressionProviders
{
    public interface IAttributeSqlExpressionProvider
    {
        string Create(Attribute attribute);

        string Rename(Attribute attribute, string newValidName);

        string Update(Attribute attribute);

        string Delele(Attribute attribute);

        string FullDefinition(Attribute attribute);
    }
}