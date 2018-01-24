using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Repositories;
using Domain.Services.Validators;
using Attribute = Domain.Entities.Attribute.Attribute;
using String = Domain.Entities.Attribute.String;

namespace Domain.Services.OfEntity
{
    public class AttributeService : IAttributeService
    {
        private const int MaxSlaveTableLinks = 10;

        private readonly IRepository<Table> _tableRepository;
        private readonly IRepository<Attribute> _attributeRepository;

        private readonly IAttributeValidator _attributeValidator;

        public AttributeService(
            IRepository<Attribute> attributeRepository,
            IAttributeValidator attributeValidator,
            IRepository<Table> tableRepository)
        {
            _attributeRepository = attributeRepository;
            _attributeValidator = attributeValidator;
            _tableRepository = tableRepository;
        }

        private void CheckAttributeName(Table table, string name)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (!_attributeValidator.IsValidName(name))
                throw new ArgumentException($"Invalid primary key name {name}.");

            if (!_attributeValidator.IsUniqueName(table: table, attributeName: name))
                throw new InvalidOperationException($"The attribute {name} is already exists.");
        }

        public int AddForeignKey(
            Table masterTable,
            Table slaveTable)
        {
            if (masterTable is null)
                throw new ArgumentNullException(nameof(masterTable));
            if (slaveTable is null)
                throw new ArgumentNullException(nameof(slaveTable));

            void GetUniqueForeignKeyName(out string localForegnKeyName)
            {
                localForegnKeyName = $"{masterTable.Name}Id";

                int uniqueForeignKeyEnding = 1;

                while (
                    !_attributeValidator.IsUniqueName(slaveTable, localForegnKeyName) &&
                    uniqueForeignKeyEnding <= MaxSlaveTableLinks)
                {
                    localForegnKeyName = $"{localForegnKeyName}{uniqueForeignKeyEnding++}";
                }

                if (uniqueForeignKeyEnding > MaxSlaveTableLinks)
                    throw new ArgumentException($"The amount of links ({MaxSlaveTableLinks}) is over.");
            }

            GetUniqueForeignKeyName(out string foregnKeyName);

            ForeignKey foreignKey = new ForeignKey(
                name: foregnKeyName,
                isNullable: true) {TableId = slaveTable.Id};

            _attributeRepository.Add(foreignKey);

            return foreignKey.Id;
        }

        public void AddPrimaryKey(
            Table table,
            string primaryKeyName)
        {
            CheckAttributeName(table: table, name: primaryKeyName);

            if (_attributeRepository
                .All()
                .Where(a => a.TableId == table.Id)
                .Any(a => a is PrimaryKey))
                throw new InvalidOperationException($"The table {table.Name} already has a primary key.");

            PrimaryKey primaryKey = new PrimaryKey(primaryKeyName) {TableId = table.Id};

            _attributeRepository.Add(primaryKey);
        }

        public void AddDecimalNumber(
            Table table,
            string name,
            TSQLType sqlType,
            int? precision = null,
            int? scale = null,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formForperties = null)
        {
            CheckAttributeName(table: table, name: name);

            DecimalNumber decimalNumber = new DecimalNumber(
                name: name,
                sqlType: sqlType,
                precision: precision,
                scale: scale,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formForperties) {TableId = table.Id};

            _attributeRepository.Add(decimalNumber);
        }

        public void AddIntegerNumber(
            Table table,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formForperties = null)
        {
            CheckAttributeName(table: table, name: name);

            IntegerNumber integer = new IntegerNumber(
                name: name,
                sqlType: sqlType,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formForperties) {TableId = table.Id};

            _attributeRepository.Add(integer);
        }

        public void AddRealNumber(
            Table table,
            string name,
            TSQLType sqlType,
            int? bitCapacity,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            string description = null,
            string formForperties = null)
        {
            CheckAttributeName(table: table, name: name);

            RealNumber realNumber = new RealNumber(
                name: name,
                sqlType: sqlType,
                bitCapacity: bitCapacity,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                description: description,
                formSettings: formForperties) {TableId = table.Id};

            _attributeRepository.Add(realNumber);
        }

        public void AddString(
            Table table,
            string name,
            TSQLType sqlType,
            bool isNullable = true,
            bool isPrimaryKey = false,
            bool isIndexed = false,
            uint? length = null,
            string defaultValue = null,
            string description = null,
            string formProperties = null)
        {
            CheckAttributeName(table: table, name: name);

            String attribute = new String(
                name: name,
                sqlType: sqlType,
                isNullable: isNullable,
                isPrimaryKey: isPrimaryKey,
                isIndexed: isIndexed,
                length: length,
                defaultValue: defaultValue,
                description: description,
                formSettings: formProperties) {TableId = table.Id};

            _attributeRepository.Add(attribute);
        }

        public IEnumerable<Attribute> GetAll()
        {
            return
                _attributeRepository
                    .All();
        }

        public void Remove(Attribute attribute)
        {
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));

            _attributeRepository
                .Remove(attribute);
        }

        public void Rename(Attribute attribute, string name)
        {
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (attribute.Name == name) return;

            if (!_attributeValidator.IsValidName(name))
                throw new ArgumentException($"Invalid primary key name {name}.");

            Table table =
                _tableRepository
                    .All()
                    .SingleOrDefault(t => t.Id == attribute.TableId);

            if (!_attributeValidator.IsUniqueName(table: table, attributeName: name))
                throw new InvalidOperationException($"The attribute {name} is already exists.");

            attribute.Rename(name);

            _attributeRepository.Update(attribute);
        }

        public Attribute GetByName(Table table, string name)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return
                _attributeRepository
                    .All()
                    .Where(a => a.TableId == table.Id)
                    .SingleOrDefault(a => a.Name == name);
        }

        public Attribute GetById(int id)
        {
            return
                _attributeRepository
                    .All()
                    .SingleOrDefault(a => a.Id == id);
        }

        public bool IsDeployable(Attribute attribute)
        {
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));

            return true;
        }

        public void OffModified(Attribute attribute)
        {
            attribute.OffModified();

            _attributeRepository.Update(attribute);
        }
    }
}