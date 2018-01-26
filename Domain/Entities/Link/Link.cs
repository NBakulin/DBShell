using System;
using Domain.Entities.Attribute.Integer;

namespace Domain.Entities.Link
{
    public class Link : Entity
    {
        protected internal Link() { }

        protected internal Link(
            PrimaryKey masterAttribute,
            ForeignKey slaveAttribute,
            bool isDeleteCascade,
            bool isUpdateCascade)
        {
            MasterAttribute = masterAttribute ?? throw new ArgumentNullException(nameof(masterAttribute));
            SlaveAttribute = slaveAttribute ?? throw new ArgumentNullException(nameof(slaveAttribute));

            IsDeleteCascade = isDeleteCascade;
            IsUpdateCascase = isUpdateCascade;
        }

        public int MasterAttributeId { get; protected internal set; }
        protected internal PrimaryKey MasterAttribute { get; protected set; }

        public int SlaveAttributeId { get; protected internal set; }
        protected internal ForeignKey SlaveAttribute { get; protected set; }

        public bool IsDeleteCascade { get; protected set; }
        public bool IsUpdateCascase { get; protected set; }
    }
}