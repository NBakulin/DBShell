using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Validators;

namespace Domain.Services.OfEntity
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IRepository<Database> _databaseRepository;
        private readonly IDatabaseValidator _databaseValidator;

        private readonly ITableService _tableService;

        public DatabaseService(
            IRepository<Database> databaseRepository,
            IDatabaseValidator databaseValidator,
            ITableService tableService)
        {
            _databaseRepository = databaseRepository;
            _databaseValidator = databaseValidator;
            _tableService = tableService;
        }

        public bool IsDatabaseExist(string databaseName)
        {
            return
                _databaseRepository
                    .All()
                    .Any(db => db.Name == databaseName);
        }

        public void Add(string databaseName, string serverName)
        {
            if (databaseName is null)
                throw new ArgumentNullException(nameof(databaseName));
            if (serverName is null)
                throw new ArgumentNullException(nameof(serverName));

            if (!_databaseValidator.IsValidName(name: databaseName))
                throw new ArgumentException($"Invalid database databaseName {databaseName}.", nameof(databaseName));

            if (!_databaseValidator.IsUniqueName(name: databaseName))
                throw new ArgumentException($"Database with databaseName {databaseName} is already exists.", nameof(databaseName));

            if (!_databaseValidator.IsValidServerName(serverName: serverName))
                throw new ArgumentException($"Invalid server databaseName {serverName}.", nameof(serverName));

            Database database = new Database(name: databaseName, serverName: serverName);

            _databaseRepository.Add(entity: database);
        }

        public Database GetByName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return
                _databaseRepository
                    .All()
                    .SingleOrDefault(db => db.Name == name);
        }

        public Database GetById(int id)
        {
            return
                _databaseRepository
                    .All()
                    .SingleOrDefault(db => db.Id == id);
        }

        public IEnumerable<Database> GetAll()
        {
            return _databaseRepository.All();
        }

        public void Rename(Database database, string databaseName)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));
            if (databaseName is null)
                throw new ArgumentNullException(nameof(databaseName));

            database.Rename(name: databaseName);

            _databaseRepository.Update(entity: database);
        }

        public void OffModified(Database database)
        {
            database.OffModified();

            _databaseRepository.Update(entity: database);
        }

        public void Remove(Database database)
        {
            // Firstly need to remove all related links because cascade delete is disabled on it!
            _tableService
                .GetDatabaseLinks(database: database)
                .ToList()
                .ForEach(l => _tableService.RemoveLink(tink: l));

            _databaseRepository.Remove(entity: database);
        }

        public bool IsDeployable(Database entity)
        {
            return true;
        }
    }
}