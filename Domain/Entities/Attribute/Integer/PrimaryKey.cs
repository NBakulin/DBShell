using System.Collections.Generic;
using Domain.Services.Validators;

namespace Domain.Entities.Attribute.Integer
{
    public class PrimaryKey : IntegerNumber
    {
        public IList<Link.Link> Links { get; protected set; }

        protected internal PrimaryKey() { }

        protected internal PrimaryKey(
            IAttributeValidator validator,
            string name)
            : base(
                validator: validator,
                name: name,
                sqlType: TSQLType.INT,
                isNullable: false,
                isPrimaryKey: true,
                isIndexed: true)
        {
            Links = new List<Link.Link>();
        }
    }
}