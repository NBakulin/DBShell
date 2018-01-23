using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Services.Validators;

namespace Domain.Entities.Attribute
{
    public abstract class Attribute : Entity
    {
        private readonly IAttributeValidator _attributeValidator;

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

        public int TableId { get; protected set; }
        protected internal Table Table { get; protected set; }


        protected Attribute() { }

        protected Attribute(
            IAttributeValidator validator,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formSettings = null)
        {
            _attributeValidator = validator;

            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (!_attributeValidator.IsValidName(name))
                throw new ArgumentException("Wrong \"Name\" argument.");
            Name = name;
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

            if (!_attributeValidator.IsValidName(name))
                throw new ArgumentException("Wrong \"Name\" argument.");

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
            if (!_attributeValidator.IsValidType(GetType(), type))
                throw new ArgumentException("Wrong \"Type\" argument.");

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