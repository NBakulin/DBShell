using Domain.Entities.Attribute.Integer;

namespace Domain.Entities.Link
{
    public class Link : Entity
    {
        public int MasterAttributeId { get; protected internal set; }
        protected internal PrimaryKey MasterAttribute { get; protected set; }

        public int SlaveAttributeId { get; protected internal set; }
        protected internal ForeignKey SlaveAttribute { get; protected set; }

        public bool IsDeleteCascade { get; protected set; }
        public bool IsUpdateCascase { get; protected set; }

        protected internal Link() { }

        protected internal Link(
            PrimaryKey masterAttribute,
            ForeignKey slaveAttribute,
            bool isDeleteCascade,
            bool isUpdateCascade)
        {
            MasterAttribute = masterAttribute ?? throw new System.ArgumentNullException(nameof(masterAttribute));
            SlaveAttribute = slaveAttribute ?? throw new System.ArgumentNullException(nameof(slaveAttribute));

            IsDeleteCascade = isDeleteCascade;
            IsUpdateCascase = isUpdateCascade;
        }
    }
}