using System;
using System.ComponentModel.DataAnnotations;
using Domain.Services.Validators;

namespace Domain.Entities.Attribute
{
    public class String : Attribute
    {
        private const int MaxStringLength = 4000;

        [Range(0, MaxStringLength)]
        public uint? Length { get; set; }

        public string DefaultValue { get; set; }

        protected internal String() { }

        protected internal String(
            IAttributeValidator validator,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            uint? length = null,
            string defaultValue = null,
            string description = null,
            string formSettings = null)
            : base(
                validator: validator,
                name: name,
                sqlType: sqlType,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formSettings)
        {
            ChangeMaxLength(length);
            DefaultValue = defaultValue;
        }

        public void ChangeMaxLength(uint? length)
        {
            if (length is null)
            {
                Length = null;
                return;
            }

            if (length < 1 ||
                length > MaxStringLength)
                throw new ArgumentOutOfRangeException(
                    $"\"Max Length\" cannot be less than 1 or more than {MaxStringLength}.");

            Length = length;
        }
    }
}