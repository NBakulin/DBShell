using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Attribute
{
    public class RealNumber : Attribute
    {
        private const int DefaultBitCapacity = 53;
        private const int MaxBitCapacity = 53;
        private const int MinBitCapacity = 1;

        [Range(MinBitCapacity, MaxBitCapacity)]
        public int? BitCapacity { get; protected set; }


        protected internal RealNumber() { }

        protected internal RealNumber(
            string name,
            TSQLType sqlType,
            int? bitCapacity = null,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formSettings = null)
            : base(
                name: name,
                sqlType: sqlType,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formSettings)
        {
            ChangeBitCapacity(bitCapacity);
        }

        public void ChangeBitCapacity(int? bitCapacity)
        {
            BitCapacity = bitCapacity ?? DefaultBitCapacity;
            if (bitCapacity < MinBitCapacity ||
                bitCapacity > MaxBitCapacity)
                throw new ArgumentOutOfRangeException(
                    $"\"Bit Capacity\" cannot be less than {MinBitCapacity} or nore than {MaxBitCapacity}.");
        }
    }
}