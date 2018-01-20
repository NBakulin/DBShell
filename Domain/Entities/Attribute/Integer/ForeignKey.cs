using Domain.Services.Validators;

namespace Domain.Entities.Attribute.Integer
{
    public class ForeignKey : IntegerNumber
    {
        public Link.Link Link { get; protected set; }


        protected internal ForeignKey() { }

        protected internal ForeignKey(
            IAttributeValidator validator,
            string name,
            bool isNullable)
            : base(
                validator: validator,
                name: name,
                sqlType: TSQLType.INT,
                isNullable: isNullable,
                isPrimaryKey: false,
                isIndexed: true) { }
    }
}