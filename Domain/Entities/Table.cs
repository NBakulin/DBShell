using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _Attribute = Domain.Entities.Attribute.Attribute;

namespace Domain.Entities
{
    public class Table : Entity
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; protected set; }

        protected internal int DatabaseId { get; protected set; }
        protected internal Database Database { get; protected set; }

        protected internal IList<_Attribute> Attributes { get; protected set; }


        protected internal Table() { }

        protected internal Table(string name)
        {
            Rename(name);

            Attributes = new List<_Attribute>();
        }

        protected internal void Rename(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        protected internal void AddAttribute(_Attribute newAttribute)
        {
            if (newAttribute == null)
                throw new ArgumentNullException(nameof(newAttribute));

            Attributes.Add(newAttribute);
        }

        protected internal void RemoveAttribute(_Attribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            Attributes.Remove(attribute);
        }
    }
}