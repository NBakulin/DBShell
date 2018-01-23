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

        [MaxLength(64)]
        public string DeployName { get; protected set; }

        public int DatabaseId { get; protected set; }
        protected internal Database Database { get; protected set; }

        protected internal IList<_Attribute> Attributes { get; protected set; }


        protected internal Table() { }

        protected internal Table(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DeployName = Name;

            Attributes = new List<_Attribute>();

            OnModified();
        }

        protected internal void Rename(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            {
                if (!IsModified)
                {
                    DeployName = Name;
                }

                Name = name;
            }

            OnModified();
        }

        protected internal void AddAttribute(_Attribute newAttribute)
        {
            if (newAttribute is null)
                throw new ArgumentNullException(nameof(newAttribute));

            Attributes.Add(newAttribute);
        }

        protected internal void RemoveAttribute(_Attribute attribute)
        {
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));

            Attributes.Remove(attribute);
        }
    }
}