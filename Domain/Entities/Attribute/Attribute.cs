using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Attribute
{
    public abstract class Attribute : Entity
    {
        protected Attribute() { }

        protected Attribute(
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formSettings = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            ChangeType(type: sqlType);

            ChangeIsNullable(isNullable: isNullable);
            ChangeIsPrimaryKey(isPrimaryKey: isPrimaryKey);
            ChangeIsIndexed(isIndexed: isIndexed);

            ChangeDescriotion(description: description);
            ChangeFormSettings(formSettings: formSettings);
        }

        [Required]
        [MaxLength(length: 64)]
        public string Name { get; protected set; }

        [Required]
        public TSQLType SqlType { get; protected set; }

        [Required]
        public bool IsNullable { get; protected set; }

        [Required]
        public bool IsPrimaryKey { get; protected set; }

        public bool IsIndexed { get; protected set; }

        [MaxLength(length: 256)]
        public string Description { get; protected set; }

        [MaxLength(length: 1024)]
        [Column(TypeName = "xml")]
        public string FormSettings { get; protected set; }

        public int TableId { get; protected internal set; }
        protected internal Table Table { get; protected set; }

        public void Rename(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            OnModified();
        }

        public void ChangeType(TSQLType type)
        {
            SqlType = type;

            OnModified();
        }

        public void ChangeIsNullable(bool isNullable)
        {
            IsNullable = isNullable;

            OnModified();
        }

        public void ChangeIsPrimaryKey(bool isPrimaryKey)
        {
            IsPrimaryKey = isPrimaryKey;

            OnModified();
        }

        public void ChangeIsIndexed(bool isIndexed)
        {
            IsIndexed = isIndexed;

            OnModified();
        }

        public void ChangeDescriotion(string description)
        {
            Description = description;

            OnModified();
        }

        public void ChangeFormSettings(string formSettings)
        {
            FormSettings = formSettings;

            OnModified();
        }
    }
}