using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Attribute
{
    public abstract class Attribute : Entity
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; protected set; }

        [MaxLength(64)]
        public string DeployName { get; protected set; }

        [Required]
        public TSQLType SqlType { get; protected set; }

        [Required]
        public bool IsNullable { get; protected set; }

        [Required]
        public bool IsPrimaryKey { get; protected set; }

        public bool IsIndexed { get; protected set; }

        [MaxLength(256)]
        public string Description { get; protected set; }

        [MaxLength(1024)]
        [Column(TypeName = "xml")]
        public string FormSettings { get; protected set; }

        public int TableId { get; protected internal set; }
        protected internal Table Table { get; protected set; }


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
            DeployName = Name;

            ChangeType(sqlType);

            ChangeIsNullable(isNullable);
            ChangeIsPrimaryKey(isPrimaryKey);
            ChangeIsIndexed(isIndexed);

            ChangeDescriotion(description);
            ChangeFormSettings(formSettings);
        }

        public void Rename(string name)
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