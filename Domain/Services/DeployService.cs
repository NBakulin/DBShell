using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Services.ExpressionProviders;
using Domain.Services.OfEntity;
using Attribute = Domain.Entities.Attribute.Attribute;

namespace Domain.Services
{
    public sealed class DeployService : IDeployService
    {
        private readonly IAttributeService _attributeService;
        private readonly IAttributeSqlExpressionProvider _attributeSqlExpressionProvider;
        private readonly IDatabaseService _databaseService;
        private readonly IDatabaseSqlExpressionProvider _databaseSqlExpressionProvider;
        private readonly IDeploySqlExpressionProvider _deploySqlExpressionProvider;
        private readonly ILinkService _linkService;
        private readonly ILinkSqlExpressionProvider _linkSqlExpressionProvider;
        private readonly ISqlExpressionExecutor _sqlExecutor;
        private readonly ITableService _tableService;
        private readonly ITableSqlExpressionProvider _tableSqlExpressionProvider;

        public DeployService(
            ISqlExpressionExecutor sqlExecutor,
            IDatabaseSqlExpressionProvider databaseSqlExpressionProvider,
            ITableSqlExpressionProvider tableSqlExpressionProvider,
            IAttributeSqlExpressionProvider attributeSqlExpressionProvider,
            ILinkSqlExpressionProvider linkSqlExpressionProvider,
            IDatabaseService databaseService,
            ITableService tableService,
            IAttributeService attributeService,
            ILinkService linkService,
            IDeploySqlExpressionProvider deploySqlExpressionProvider)
        {
            _sqlExecutor = sqlExecutor;
            _databaseSqlExpressionProvider = databaseSqlExpressionProvider;
            _tableSqlExpressionProvider = tableSqlExpressionProvider;
            _attributeSqlExpressionProvider = attributeSqlExpressionProvider;
            _linkSqlExpressionProvider = linkSqlExpressionProvider;
            _databaseService = databaseService;
            _tableService = tableService;
            _attributeService = attributeService;
            _linkService = linkService;
            _deploySqlExpressionProvider = deploySqlExpressionProvider;
        }


        public bool IsDeployed(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsSimilarNamedDatabaseExists(localDatabase: database))
                return false;

            if (!IsSimilarNamedTablesExist(localDatabase: database))
                return false;

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!IsSimilarAttributesExist(localDatabase: database))
                return false;

            return true;

            bool IsSimilarNamedDatabaseExists(Database localDatabase)
            {
                return
                    _sqlExecutor.ExecuteScalarAsDefault<int>(
                        serverName: localDatabase.ServerName,
                        sqlExpression: _deploySqlExpressionProvider
                            .SameNamedDatabaseCount(database: localDatabase)) == 1;
            }

            bool IsSimilarNamedTablesExist(Database localDatabase)
            {
                return
                    _sqlExecutor
                        .ExecuteReader<string>(
                            connectionString: localDatabase.ConnectionString,
                            sqlExpression: _deploySqlExpressionProvider.DatabaseTablesNames(database: localDatabase))
                        .OrderBy(tableName => tableName)
                        .SequenceEqual(
                            _tableService
                                .GetDatabaseTables(database: localDatabase)
                                .Select(t => t.Name)
                                .OrderBy(tableName => tableName));
            }

            bool IsSimilarAttributesExist(Database localDatabase)
            {
                IEnumerable<Table> tables = _tableService.GetDatabaseTables(database: localDatabase).ToList();

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Table table in tables)
                {
                    IEnumerable<string> deployedAttributeNames =
                        _sqlExecutor
                            .ExecuteReader<string>(
                                connectionString: localDatabase.ConnectionString,
                                sqlExpression: _deploySqlExpressionProvider
                                    .SameNamedAttributes(database: localDatabase, table: table))
                            .OrderBy(tableName => tableName)
                            .ToList();

                    IEnumerable<string> metadataAtrributeNames =
                        _tableService
                            .GetTableAttributes(table: table)
                            .Select(t => t.Name)
                            .OrderBy(tableName => tableName)
                            .ToList();

                    if (!deployedAttributeNames.SequenceEqual(second: metadataAtrributeNames))
                        return false;
                }

                return true;
            }
        }

        public bool IsDeployPossible(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsDatabaseDeployPossible(localDatabase: database))
                return false;

            IEnumerable<Table> tablesQuery = _tableService.GetDatabaseTables(database: database);

            if (tablesQuery is null)
                return true;

            IEnumerable<Table> tables = tablesQuery.ToList();

            if (!IsTablesDeployPossible(localTables: tables))
                return false;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Table table in tables)
            {
                IEnumerable<Attribute> tableAttributes = _tableService.GetTableAttributes(table: table);

                if (!IsAttributesDeployPossible(localAttributes: tableAttributes))
                    return false;
            }

            return true;

            bool IsDatabaseDeployPossible(Database localDatabase)
            {
                return _databaseService.IsDeployable(entity: localDatabase);
            }

            bool IsTablesDeployPossible(IEnumerable<Table> localTables)
            {
                return localTables.All(predicate: _tableService.IsDeployable);
            }

            bool IsAttributesDeployPossible(IEnumerable<Attribute> localAttributes)
            {
                return localAttributes.All(predicate: _attributeService.IsDeployable);
            }
        }

        public void Deploy(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsDeployPossible(database: database))
                throw new ArgumentException(database.ToString(), $"Database {database.Name} cannot be deployed.");

            if (IsDeployed(database: database))
                throw new ArgumentException(database.ToString(), $"Database {database.Name} was deployed before.");

            DeployDatabase(localDatabase: database);

            DeployDatabaseTablesWithAttributes(localDatabase: database);

            DeployDatabaseLinks(localDatabase: database);

            // ReSharper disable once RedundantJumpStatement
            return;

            void DeployDatabase(Database localDatabase)
            {
                _sqlExecutor
                    .ExecuteAsDefault(
                        serverName: localDatabase.ServerName,
                        sqlExpression: _databaseSqlExpressionProvider
                            .Create(database: localDatabase));

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
                                .CreateFull(table: table));

                        _tableService
                            .GetTableAttributes(table: table)
                            .ToList()
                            .ForEach(attribute => _attributeService
                                         .OffModified(attribute: attribute));

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
                                         .Create(link: link)));
            }
        }

        public void UpdateDeployed(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            if (!IsDeployed(database: database))
                throw new ArgumentException(database.ToString(), $"Database {database.Name} cannot be updated because it is not deployed.");

            UpdateDatabaseAttributes(localDatabase: database);

            UpdateDatabaseTables(localDatabase: database);

            UpdateDatabase(localDatabase: database);

            // ReSharper disable once RedundantJumpStatement
            return;

            void UpdateDatabase(Database localDatabase)
            {
                if (!localDatabase.IsModified) return;

                _sqlExecutor.Execute(
                    sqlConnectionString: localDatabase.ConnectionString,
                    sqlExpression: _databaseSqlExpressionProvider
                        .Update(database: localDatabase));

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
                                .Update(table: table));

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
                                     .GetTableAttributes(table: table)
                                     .ToList()
                                     .Where(a => a.IsModified)
                                     .ToList()
                                     .ForEach(attribute =>
                                     {
                                         _sqlExecutor.Execute(
                                             sqlConnectionString: localDatabase.ConnectionString,
                                             sqlExpression: _attributeSqlExpressionProvider
                                                 .Update(attribute: attribute));

                                         _attributeService.OffModified(attribute: attribute);
                                     }));
            }
        }

        public void RenameDeployedDatabase(Database database, string validNewDatabaseName)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));
            if (validNewDatabaseName is null)
                throw new ArgumentNullException(nameof(validNewDatabaseName));

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _databaseSqlExpressionProvider
                    .Rename(database: database, validNewName: validNewDatabaseName));
        }

        public void DropDeployedDatabase(Database database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            _sqlExecutor.ExecuteAsDefault(
                serverName: database.ServerName,
                sqlExpression: _databaseSqlExpressionProvider
                    .Remove(database: database));
        }

        public void RenameDeployedTable(Table table, string validNewTableName)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (validNewTableName is null)
                throw new ArgumentNullException(nameof(validNewTableName));

            Database database = _databaseService.GetById(id: table.DatabaseId);

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _tableSqlExpressionProvider
                    .Rename(table: table, newValidName: validNewTableName));
        }

        public void DropDeployedTable(Table table)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));

            Database database = _databaseService.GetById(id: table.DatabaseId);

            IEnumerable<Attribute> attributesQuery = _tableService.GetTableAttributes(table: table);

            IEnumerable<Attribute> attributes = attributesQuery.ToList();

            PrimaryKey primaryKey = attributes.OfType<PrimaryKey>().Single();

            _linkService
                .GetDatabaseLinks(database: database)
                .Where(l => l.MasterAttributeId == primaryKey.Id)
                .ToList()
                .ForEach(action: DropDeployedLink);

            IEnumerable<ForeignKey> foreignKeys = attributes.OfType<ForeignKey>();

            _linkService
                .GetDatabaseLinks(database: database)
                .ToList()
                .ForEach(l =>
                {
                    if (foreignKeys.Any(fk => fk.Id == l.SlaveAttributeId)) DropDeployedLink(link: l);
                });

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _tableSqlExpressionProvider
                    .Remove(table: table));
        }

        public void RenameDeployedAttribute(Attribute attribute, string newAttributeName)
        {
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));
            if (newAttributeName is null)
                throw new ArgumentNullException(nameof(newAttributeName));

            Table table = _tableService.GetTableById(tableId: attribute.TableId);

            Database database = _databaseService.GetById(id: table.DatabaseId);

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _attributeSqlExpressionProvider
                    .Rename(attribute: attribute, newValidName: newAttributeName));
        }

        public void DropDeployedAttribute(Attribute attribute)
        {
            switch (attribute)
            {
                case null:
                    throw new ArgumentNullException(nameof(attribute));
                case PrimaryKey _:
                case ForeignKey _:
                    throw new ArgumentException($"The attribute {attribute.Name} is primary or foreign key.");
            }

            Table table = _tableService.GetTableById(tableId: attribute.TableId);

            Database database = _databaseService.GetById(id: table.DatabaseId);

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _attributeSqlExpressionProvider
                    .Delele(attribute: attribute));
        }

        public void DropDeployedLink(Link link)
        {
            if (link is null)
                throw new ArgumentNullException(nameof(link));

            if (!(_attributeService.GetById(id: link.SlaveAttributeId) is ForeignKey foreignKey))
                throw new NullReferenceException($"The link {link.Id} matches empty foreign key attribute.");

            Table table = _tableService.GetTableById(tableId: foreignKey.TableId);

            Database database = _databaseService.GetById(id: table.DatabaseId);

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _attributeSqlExpressionProvider
                    .Delele(attribute: foreignKey));

            _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _linkSqlExpressionProvider
                    .Remove(link: link));
        }
    }
}