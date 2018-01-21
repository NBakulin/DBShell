using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Link;
using Domain.Repositories;
using Domain.Services.ExpressionProviders;
using Domain.Services.OfEntity;
using Attribute = Domain.Entities.Attribute.Attribute;

namespace Domain.Services
{
    public sealed class DeployService : IDeployService
    {
        private readonly ISqlExpressionExecutor _sqlExecutor;

        private readonly IDatabaseSqlExpressionProvider _databaseSqlExpressionProvider;
        private readonly ITableSqlExpressionProvider _tableSqlExpressionProvider;
        private readonly IAttributeSqlExpressionProvider _attributeSqlExpressionProvider;
        private readonly ILinkSqlExpressionProvider _linkSqlExpressionProvider;

        private readonly IRepository<Database> _databaseRepository;
        private readonly IRepository<Table> _tableRepository;
        private readonly IRepository<Attribute> _attributeRepository;
        private readonly IRepository<Link> _linkRepository;

        private readonly IDatabaseService _databaseService;
        private readonly ITableService _tableService;
        private readonly IAttributeService _attributeService;

        public DeployService(
            ISqlExpressionExecutor sqlExecutor,
            IDatabaseSqlExpressionProvider databaseSqlExpressionProvider,
            ITableSqlExpressionProvider tableSqlExpressionProvider,
            IAttributeSqlExpressionProvider attributeSqlExpressionProvider,
            ILinkSqlExpressionProvider linkSqlExpressionProvider,
            IRepository<Database> databaseRepository,
            IRepository<Table> tableRepository,
            IRepository<Attribute> attributeRepository,
            IRepository<Link> linkRepository,
            IDatabaseService databaseService,
            ITableService tableService,
            IAttributeService attributeService)
        {
            _sqlExecutor = sqlExecutor;
            _databaseSqlExpressionProvider = databaseSqlExpressionProvider;
            _tableSqlExpressionProvider = tableSqlExpressionProvider;
            _attributeSqlExpressionProvider = attributeSqlExpressionProvider;
            _linkSqlExpressionProvider = linkSqlExpressionProvider;
            _databaseRepository = databaseRepository;
            _tableRepository = tableRepository;
            _attributeRepository = attributeRepository;
            _linkRepository = linkRepository;
            _databaseService = databaseService;
            _tableService = tableService;
            _attributeService = attributeService;
        }


        public bool IsDeployed(Database database)
        {
            // Check for database with the same name

            string similarDatabaseName =
                "SELECT COUNT(*) \n" +
                "FROM sys.databases \n" +
                $"WHERE name = '{database.DeployName}'";

            bool isSimilarNamedDatabaseExists =
                _sqlExecutor.ExecuteScalarAsDefault<int>(database.ServerName, similarDatabaseName) == 1;

            if (!isSimilarNamedDatabaseExists)
                return false;

            // Check for existing database tables

            string similarTableNames =
                "SELECT TABLE_NAME \n" +
                $"FROM [{database.DeployName}].[INFORMATION_SCHEMA].[TABLES]";

            bool isSimilarTableNames =
                _sqlExecutor
                    .ExecuteReader<string>(database.ConnectionString, similarTableNames)
                    .OrderBy(tableName => tableName)
                    .SequenceEqual(
                        _tableService
                            .GetDatabaseTables(database)
                            .Select(t => t.DeployName)
                            .OrderBy(tableName => tableName));

            // TODO check the other parameters

            if (!isSimilarTableNames)
                return false;

            return true;
        }

        public bool IsDeployPossible(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            return
                (from db in _databaseRepository.All()
                    join t in _tableRepository.All() on db.Id equals t.DatabaseId
                    join a in _attributeRepository.All() on t.Id equals a.TableId
                    select new {db, t, a})
                .ToList()
                .Any(tuple =>
                    _databaseService.IsDeployable(tuple.db) &&
                    _tableService.IsDeployable(tuple.t) &&
                    _attributeService.IsDeployable(tuple.a));
        }

        public void Deploy(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsDeployPossible(database))
                throw new ArgumentException(database.ToString(), $"Database {database.DeployName} cannot be deployed.");

            if (IsDeployed(database))
                throw new ArgumentException(database.ToString(), $"Database {database.DeployName} was deployed before.");

            DeployDatabase(localDatabase: database);

            DeployDatabaseTablesWithAttributes(localDatabase: database);

            DeployDatabaseLinks(localDatabase: database);

            return;

            void DeployDatabase(Database localDatabase)
            {
                _sqlExecutor
                    .ExecuteAsDefault(
                        serverName: localDatabase.ServerName,
                        sqlExpression: _databaseSqlExpressionProvider
                            .Create(localDatabase));

                _databaseService.OffModified(database: database);
            }

            void DeployDatabaseTablesWithAttributes(Database localDatabase)
            {
                _tableService
                    .GetDatabaseTables(database: localDatabase)
                    .ToList()
                    .ForEach(table =>
                    {
                        _sqlExecutor.Execute(
                            sqlConnectionString: localDatabase.ConnectionString,
                            sqlExpression: _tableSqlExpressionProvider
                                .CreateFull(table));

                        _tableService
                            .GetTableAttributes(table: table)
                            .ToList()
                            .ForEach(attribute => _attributeService
                                .OffModified(attribute));

                        _tableService
                            .OffModified(table: table);
                    });
            }

            void DeployDatabaseLinks(Database localDatabase)
            {
                _tableService
                    .GetDatabaseLinks(database: localDatabase)
                    .ToList()
                    .ForEach(link =>
                        _sqlExecutor.Execute(
                            sqlConnectionString: localDatabase.ConnectionString,
                            sqlExpression: _linkSqlExpressionProvider
                                .Create(link)));
            }
        }

        public void UpdateDeployed(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsDeployed(database))
                throw new ArgumentException(database.ToString(), $"Database {database.DeployName} cannot be updated because it is not deployed.");

            UpdateDatabaseAttributes(localDatabase: database);

            UpdateDatabaseTables(localDatabase: database);

            UpdateDatabase(localDatabase: database);

            return;

            void UpdateDatabase(Database localDatabase)
            {
                if (!localDatabase.IsModified) return;

                _sqlExecutor.Execute(
                    sqlConnectionString: localDatabase.ConnectionString,
                    sqlExpression: _databaseSqlExpressionProvider
                        .UpdateName(database: localDatabase));

                _databaseService.OffModified(database: localDatabase);
            }

            void UpdateDatabaseTables(Database localDatabase)
            {
                _tableService
                    .GetDatabaseTables(database: localDatabase)
                    .ToList()
                    .Where(t => t.IsModified)
                    .ToList()
                    .ForEach(table =>
                    {
                        _sqlExecutor.Execute(
                            sqlConnectionString: localDatabase.ConnectionString,
                            sqlExpression: _tableSqlExpressionProvider
                                .Update(table));

                        _tableService.OffModified(table: table);
                    });
            }

            void UpdateDatabaseAttributes(Database localDatabase)
            {
                _tableService
                    .GetDatabaseTables(database: localDatabase)
                    .ToList()
                    .ForEach(table =>
                        _tableService
                            .GetTableAttributes(table)
                            .ToList()
                            .Where(a => a.IsModified)
                            .ToList()
                            .ForEach(attribute =>
                            {
                                _sqlExecutor.Execute(
                                    sqlConnectionString: localDatabase.ConnectionString,
                                    sqlExpression: _attributeSqlExpressionProvider
                                        .Update(attribute));

                                _attributeService.OffModified(attribute: attribute);
                            }));
            }
        }

        public void DropDeployed(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            _sqlExecutor.ExecuteAsDefault(
                serverName: database.ServerName,
                sqlExpression: _databaseSqlExpressionProvider
                    .Remove(database));
        }
    }
}