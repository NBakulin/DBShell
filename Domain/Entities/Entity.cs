using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; protected set; }
    }
}