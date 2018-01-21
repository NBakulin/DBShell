using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; protected set; }

        public bool IsModified { get; private set; }

        protected internal void OnModified()
        {
            IsModified = true;
        }

        protected internal void OffModified()
        {
            IsModified = false;
        }
    }
}