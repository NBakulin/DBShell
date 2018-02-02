using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Repositories;
using Domain.Services.Validators;
using Attribute = Domain.Entities.Attribute.Attribute;

namespace Domain.Services.OfEntity
{
    public class TableService : ITableService
    {
        private readonly IRepository<Attribute> _attributeRepository;
        private readonly IAttributeService _attributeService;
        private readonly ILinkService _linkService;
        private readonly IRepository<Table> _tableRepository;
        private readonly IRepository<Link> _linkRepository;
        private readonly ITableValidator _tableValidator;


        public TableService(
            IRepository<Table> tableRepository,
            IAttributeService attributeService,
            ITableValidator tableValidator,
            ILinkService linkService,
            IRepository<Attribute> attributeRepository,
            IRepository<Link> linkRepository)
        {
            _tableRepository = tableRepository;
            _attributeService = attributeService;
            _tableValidator = tableValidator;
            _linkService = linkService;
            _attributeRepository = attributeRepository;
            _linkRepository = linkRepository;
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

            Table table = new Table(name: tableName) {DatabaseId = database.Id};

            _tableRepository.Add(entity: table);

            _attributeService.AddPrimaryKey(table: table, primaryKeyName: "Id");
        }

        public void Rename(Table table, string tableName)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (tableName is null)
                throw new ArgumentNullException(nameof(tableName));

            table.Rename(name: tableName);

            _tableRepository.Update(entity: table);
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
            return _linkService.GetDatabaseLinks(database: database);
        }

        public IEnumerable<Attribute> GetTableAttributes(Table table)
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
            PrimaryKey primaryKey = _linkService.GetPrimaryKey(table: table);

            _linkRepository
                .All()
                .Where(l => l.MasterAttributeId == primaryKey.Id)
                .ToList()
                .ForEach(l => _linkService.Remove(link: l));

            _tableRepository.Remove(entity: table);
        }

        public void RemoveLink(Link link)
        {
            _linkService.Remove(link: link);
        }

        public bool HasPrimaryKey(Table table)
        {
            return
                GetTableAttributes(table: table)
                    .Any(a => a is PrimaryKey);
        }

        public bool IsDeployable(Table table)
        {
            return HasPrimaryKey(table: table);
        }

        public void OffModified(Table table)
        {
            table.OffModified();

            _tableRepository.Update(entity: table);
        }
    }
}