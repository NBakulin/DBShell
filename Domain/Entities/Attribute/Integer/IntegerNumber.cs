using Domain.Services.Validators;

namespace Domain.Entities.Attribute.Integer
{
    public class IntegerNumber : Attribute
    {
        protected internal IntegerNumber() { }

        protected internal IntegerNumber(
            IAttributeValidator validator,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formSettings = null)
            : base(
                validator: validator,
                name: name,
                sqlType: sqlType,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formSettings) { }
    }
}