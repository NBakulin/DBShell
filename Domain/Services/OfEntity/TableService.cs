using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Repositories;
using Domain.Services.Validators;

namespace Domain.Services.OfEntity
{
    public class TableService : ITableService
    {
        private readonly IRepository<Table> _tableRepository;
        private readonly IRepository<Database> _databaseRepository;
        private readonly IRepository<Entities.Attribute.Attribute> _attributeRepository;
        private readonly ITableValidator _tableValidator;

        private readonly IAttributeService _attributeService;
        private readonly ILinkService _linkService;

        public TableService(
            IRepository<Table> tableRepository,
            IAttributeService attributeService,
            ITableValidator tableValidator,
            ILinkService linkService,
            IRepository<Database> databaseRepository,
            IRepository<Entities.Attribute.Attribute> attributeRepository)
        {
            _tableRepository = tableRepository;
            _attributeService = attributeService;
            _tableValidator = tableValidator;
            _linkService = linkService;
            _databaseRepository = databaseRepository;
            _attributeRepository = attributeRepository;
        }

        public void Add(Database database, string tableName)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));
            if (tableName is null)
                throw new ArgumentNullException(nameof(tableName));

            if (!_tableValidator.IsValidName(tableName: tableName))
                throw new ArgumentException($"Invalid table name {tableName}.");

            if (!_tableValidator.IsUniqueName(database: database, tableName: tableName))
                throw new ArgumentException($"The table {tableName} is already exists in the database {database.Name}.");

            Table table = new Table(tableName);

            database.AddTable(table);

            _tableRepository.Add(table);

            _attributeService.AddPrimaryKey(table, "Id");
        }

        public void Rename(Table table, string tableName)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (tableName is null)
                throw new ArgumentNullException(nameof(tableName));

            if (!_tableValidator.IsValidName(tableName: tableName))
                throw new ArgumentException($"Invalid table name {tableName}.");

            Database database =
                _databaseRepository
                    .All()
                    .SingleOrDefault(db => db.Id == table.DatabaseId);

            if (!_tableValidator.IsUniqueName(database: database, tableName: tableName))
                throw new ArgumentException($"The table {tableName} is already exists in the database {database.Name}.");

            table.Rename(tableName);

            _tableRepository.Update(table);
        }

        public IEnumerable<Table> GetDatabaseTables(Database database)
        {
            return
                _tableRepository
                    .All()
                    .Where(t => t.DatabaseId == database.Id);
        }

        public IEnumerable<Link> GetDatabaseLinks(Database database)
        {
            return _linkService.GetAll(database: database);
        }

        public IEnumerable<Entities.Attribute.Attribute> GetTableAttributes(Table table)
        {
            return
                _attributeRepository
                    .All()
                    .Where(a => a.TableId == table.Id);
        }

        public Table GetTableByName(Database database, string tableName)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));
            if (tableName is null)
                throw new ArgumentNullException(nameof(tableName));

            return
                _tableRepository
                    .All()
                    .Where(t => t.DatabaseId == database.Id)
                    .SingleOrDefault(t => t.Name == tableName);
        }

        public Table GetTableById(int tableId)
        {
            return
                _tableRepository
                    .All()
                    .SingleOrDefault(t => t.Id == tableId);
        }

        public void RemoveTable(Table table)
        {
            _tableRepository.Remove(table);
        }

        public void RemoveLink(Link link)
        {
            _linkService.Remove(link: link);
        }

        public bool HasPrimaryKey(Table table)
        {
            return
                GetTableAttributes(table)
                    .Any(a => a is PrimaryKey);
        }

        public bool IsDeployable(Table table)
        {
            return HasPrimaryKey(table);
        }
    }
}