using System;
using Domain.Entities;
using Domain.Entities.Attribute;

namespace Domain.Services.Validators
{
    public interface IAttributeValidator
    {
        bool IsValidName(string name);

        bool IsUniqueName(Table table, string attributeName);

        bool IsValidType(Type type, TSQLType sqlType);
    }
}