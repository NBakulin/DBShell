using System.Collections.Generic;

namespace Domain.Entities.Attribute.Integer
{
    public class PrimaryKey : IntegerNumber
    {
        public IList<Link.Link> Links { get; protected set; }

        protected internal PrimaryKey() { }

        protected internal PrimaryKey(
            string name)
            : base(
                name: name,
                sqlType: TSQLType.INT,
                isNullable: false,
                isPrimaryKey: true,
                isIndexed: true) { }
    }
}