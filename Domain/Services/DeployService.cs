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
                $"WHERE name = '{database.Name}'";

            bool isSimilarNamedDatabaseExists =
                _sqlExecutor.ExecuteScalarAsDefault<int>(database.ServerName, similarDatabaseName) == 1;

            // Check for existing database tables

            string similarTableNames =
                "SELECT TABLE_NAME \n" +
                $"FROM [{database.Name}].[INFORMATION_SCHEMA].[TABLES]";


            bool isSimilarTableNames =
                _sqlExecutor
                    .ExecuteReader<string>(database.ConnectionString, similarTableNames)
                    .SequenceEqual(
                        _tableService
                            .GetDatabaseTables(database)
                            .Select(t => t.Name));

            // TODO check the other parameters

            return isSimilarNamedDatabaseExists && isSimilarTableNames;
        }

        public bool IsDeployable(Database database)
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

        public void DeployDatabase(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsDeployable(database))
                throw new ArgumentException(database.ToString(), $"Database {database.Name} cannot be deployed.");

            string sqlConnectionString = database.ConnectionString;

            _sqlExecutor.ExecuteAsDefault(
                serverName: database.ServerName,
                sqlExpression: _databaseSqlExpressionProvider
                    .Create(database));

            _tableRepository
                .All()
                .Where(t => t.DatabaseId == database.Id)
                .ToList()
                .ForEach(table =>
                    _sqlExecutor.Execute(
                        sqlConnectionString: sqlConnectionString,
                        sqlExpression: _tableSqlExpressionProvider
                            .CreateFull(table)));


            (from link in _linkRepository.All()
                    join primaryKeyAttribute in _attributeRepository.All() on link.MasterAttribute.Id equals primaryKeyAttribute.Id
                    join table in _tableRepository.All() on primaryKeyAttribute.TableId equals table.Id
                    where table.DatabaseId == database.Id
                    select link)
                .ToList()
                .ForEach(link => _sqlExecutor.Execute(
                    sqlConnectionString: sqlConnectionString,
                    sqlExpression: _linkSqlExpressionProvider
                        .Create(link)));
        }

        public void UpgradeDatabase(Database database) { }

        public void DropDatabase(Database database)
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