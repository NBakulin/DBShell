using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public bool IsModified { get; private set; }

        [Key]
        public int Id { get; protected set; }

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