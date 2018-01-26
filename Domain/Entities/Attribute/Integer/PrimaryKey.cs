using System.Collections.Generic;

namespace Domain.Entities.Attribute.Integer
{
    public class PrimaryKey : IntegerNumber
    {
        protected internal PrimaryKey() { }

        protected internal PrimaryKey(
            string name)
            : base(
                name: name,
                sqlType: TSQLType.INT,
                isNullable: false,
                isPrimaryKey: true,
                isIndexed: true) { }

        public IList<Link.Link> Links { get; protected set; }
    }
}