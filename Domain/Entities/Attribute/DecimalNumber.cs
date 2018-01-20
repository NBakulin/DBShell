using System;
using System.ComponentModel.DataAnnotations;
using Domain.Services.Validators;

namespace Domain.Entities.Attribute
{
    public class DecimalNumber : Attribute
    {
        private const int MaxPrecision = 38;
        private const int MinPrecision = 1;

        [Range(MinPrecision, MaxPrecision)]
        public int? Precision { get; protected set; }

        [Range(0, MaxPrecision)]
        public int? Scale { get; protected set; }


        protected internal DecimalNumber() { }

        protected internal DecimalNumber(
            IAttributeValidator validator,
            string name,
            TSQLType sqlType,
            int? precision = null,
            int? scale = null,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formSettings = null)
            : base(
                validator,
                name: name,
                sqlType: sqlType,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formSettings)
        {
            ChangePrecision(precision);
            ChangeScale(scale);
        }

        public void ChangePrecision(int? precision)
        {
            Precision = precision;

            if (precision is null) return;

            if (precision < MinPrecision ||
                precision > MaxPrecision)
                throw new ArgumentOutOfRangeException(
                    $"\"Precision\" cannot be less than {MinPrecision} or more than {MaxPrecision}.");
        }

        protected void ChangeScale(int? scale)
        {
            if (scale is null || scale == 0)
            {
                Scale = null;
                return;
            }

            if (Precision is null)
                throw new NullReferenceException("Cannot set \"Scale\" when precision is not defined.");

            if (scale > Precision ||
                scale < 0)
                throw new ArgumentOutOfRangeException(scale.ToString(),
                    "\"Scale\" cannot be less than zero or more than \"Precision\".");

            Scale = scale;
        }
    }
}