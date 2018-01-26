using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _Attribute = Domain.Entities.Attribute.Attribute;

namespace Domain.Entities
{
    public class Table : Entity
    {
        protected internal Table() { }

        protected internal Table(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            OnModified();
        }

        [Required]
        [MaxLength(length: 64)]
        public string Name { get; protected set; }

        public int DatabaseId { get; protected internal set; }
        protected internal Database Database { get; protected set; }

        protected internal IList<_Attribute> Attributes { get; protected set; }

        protected internal void Rename(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            OnModified();
        }
    }
}