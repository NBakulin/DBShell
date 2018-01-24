using System.Collections.Generic;

namespace Domain.Entities.Attribute.Integer
{
    public class ForeignKey : IntegerNumber
    {
        public IList<Link.Link> Links { get; protected set; }

        protected internal ForeignKey() { }

        protected internal ForeignKey(
            string name,
            bool isNullable)
            : base(
                name: name,
                sqlType: TSQLType.INT,
                isNullable: isNullable,
                isPrimaryKey: false,
                isIndexed: true) { }
    }
}