using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.Attribute;

namespace Domain.Services.OfEntity
{
    public interface IAttributeService : IDeployable<Attribute>
    {
        int AddForeignKey(
            Table masterTable,
            Table slaveTable);

        void AddPrimaryKey(
            Table table,
            string primaryKeyName);

        void AddDecimalNumber(
            Table table,
            string name,
            TSQLType sqlType,
            int? precision = null,
            int? scale = null,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formForperties = null);

        void AddIntegerNumber(
            Table table,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formForperties = null);

        void AddRealNumber(
            Table table,
            string name,
            TSQLType sqlType,
            int? bitCapacity,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formForperties = null);

        void AddString(
            Table table,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            uint? length = null,
            string defaultValue = null,
            string description = null,
            string formProperties = null);

        IEnumerable<Attribute> GetAll();

        void Remove(Attribute attribute);

        void Rename(Attribute attribute, string name);

        Attribute GetByName(Table table, string name);

        Attribute GetById(int id);

        void OffModified(Attribute attribute);
    }
}