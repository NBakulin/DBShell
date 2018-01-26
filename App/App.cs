using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Services;
using Domain.Services.OfEntity;
using Domain.Services.Validators;
using Attribute = Domain.Entities.Attribute.Attribute;

namespace App
{
    public class App
    {
        private readonly IAttributeService _attributeService;
        private readonly IDatabaseService _databaseService;
        private readonly IDeployService _deployService;
        private readonly ILinkService _linkService;
        private readonly ITableService _tableService;

        private readonly IDatabaseValidator _databaseValidator;
        private readonly ITableValidator _tableValidator;
        private readonly IAttributeValidator _attributeValidator;

        private readonly string _defaultConnectionString;
        private readonly string _serverName;


        public App(
            string connectionString,
            IAttributeService attributeService,
            IDatabaseService databaseService,
            ITableService tableService,
            ILinkService linkService,
            IDeployService deployService,
            IDatabaseValidator databaseValidator,
            ITableValidator tableValidator,
            IAttributeValidator attributeValidator)
        {
            _defaultConnectionString = connectionString;
            // sorry for this
            _serverName = new Regex("(?:[Dd]ata\\s+[Ss]ource\\s*=\\s*)(?<server>.*?);")
                          .Match(input: _defaultConnectionString)
                          .Groups["server"]
                          .Value;

            _attributeService = attributeService;
            _databaseService = databaseService;
            _tableService = tableService;
            _linkService = linkService;
            _deployService = deployService;
            _databaseValidator = databaseValidator;
            _tableValidator = tableValidator;
            _attributeValidator = attributeValidator;
        }

        public (bool ItWorks, string ErrorMessage) IsConnectionWorks()
        {
            SqlConnection connection = new SqlConnection(connectionString: _defaultConnectionString);

            bool itWorks = false;
            string message = string.Empty;

            try
            {
                connection.Open();
                itWorks = true;
            }
            catch (SqlException e)
            {
                message = $"Database connection exception: {e.Message}.";
            }
            finally
            {
                connection.Close();
            }

            return (itWorks, message);
        }

        #region DATABASE

        public bool IsDatabaseExist(string databaseName)
        {
            return _databaseService.IsDatabaseExist(databaseName: databaseName);
        }

        public void CreateDatabase(string name)
        {
            _databaseService.Add(databaseName: name, serverName: _serverName);
        }

        public Database GetDatabaseByName(string name)
        {
            return _databaseService.GetByName(name: name);
        }

        public Database GetDatabaseById(int id)
        {
            return _databaseService.GetById(id: id);
        }

        public void RemoveDatabase(Database database)
        {
            if (_deployService.IsDeployed(database: database)) _deployService.DropDeployedDatabase(database: database);

            _databaseService.Remove(database: database);
        }

        public IEnumerable<Database> GetAllDatabases()
        {
            return _databaseService.GetAll();
        }

        public void RenameDatabase(Database database, string databaseName)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));
            if (databaseName is null)
                throw new ArgumentNullException(nameof(databaseName));

            if (database.Name == databaseName) return;

            if (!_databaseValidator.IsValidName(name: databaseName))
                throw new ArgumentException($"Invalid database tableName {databaseName}.", nameof(databaseName));

            if (!_databaseValidator.IsUniqueName(name: databaseName))
                throw new ArgumentException($"Database with tableName {databaseName} is already exists.", nameof(databaseName));

            if (_deployService.IsDeployed(database: database))
            {
                _deployService.RenameDeployedDatabase(database: database, validNewDatabaseName: databaseName);
            }

            _databaseService.Rename(database: database, name: databaseName);
        }

        #endregion

        #region TABLE

        public void AddTable(Database database, string name)
        {
            _tableService.Add(database: database, tableName: name);
        }

        public IEnumerable<Table> GetDatabaseTables(Database database)
        {
            return _tableService.GetDatabaseTables(database: database);
        }

        public Table GetTableByName(Database database, string name)
        {
            return _tableService.GetTableByName(database: database, name: name);
        }

        public Table GetTableById(int id)
        {
            return _tableService.GetTableById(tableId: id);
        }

        public void RemoveTable(Table table)
        {
            Database database = _databaseService.GetById(id: table.DatabaseId);

            if (_deployService.IsDeployed(database: database)) _deployService.DropDeployedTable(table: table);

            _tableService.RemoveTable(table: table);
        }

        public void RenameTable(Table table, string tableName)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (tableName is null)
                throw new ArgumentNullException(nameof(tableName));

            if (table.Name == tableName) return;

            if (!_tableValidator.IsValidName(tableName: tableName))
                throw new ArgumentException($"Invalid table name {tableName}.");

            Database database = _databaseService.GetById(id: table.DatabaseId);

            if (database is null)
                throw new ArgumentException($"The table {table.Name} does not belog to any database.");

            if (!_tableValidator.IsUniqueName(database: database, tableName: tableName))
                throw new ArgumentException($"The table {tableName} is already exists in the database {database.Name}.");

            if (_deployService.IsDeployed(database: database))
            {
                _deployService.RenameDeployedTable(table: table, validNewTableName: tableName);
            }

            _tableService.Rename(table: table, name: tableName);
        }

        #endregion

        #region ATTRIBUTE

        public void AddStringAttribute(
            Table table,
            string name,
            bool isNullable = true,
            uint? length = null)
        {
            _attributeService.AddString(
                table: table,
                name: name,
                sqlType: TSQLType.NVARCHAR,
                isNullable: isNullable,
                length: length);
        }

        public void AddIntegerAttribute(
            Table table,
            string name,
            bool isNullable = true)
        {
            _attributeService.AddIntegerNumber(
                table: table,
                name: name,
                sqlType: TSQLType.INT,
                isNullable: isNullable);
        }

        public void AddDecimalAttribute(
            Table table,
            string name,
            bool isNullable = true,
            int? precision = null,
            int? scale = null)
        {
            _attributeService.AddDecimalNumber(
                table: table,
                name: name,
                sqlType: TSQLType.DECIMAL,
                precision: precision,
                scale: scale,
                isNullable: isNullable);
        }

        public void AddFloatAttribute(
            Table table,
            string name,
            bool isNullable = true,
            int? bitCapacity = null)
        {
            _attributeService.AddRealNumber(
                table: table,
                name: name,
                sqlType: TSQLType.FLOAT,
                bitCapacity: bitCapacity,
                isNullable: isNullable);
        }

        public void RemoveAttribute(Attribute attribute)
        {
            Table table = _tableService.GetTableById(tableId: attribute.TableId);

            Database database = _databaseService.GetById(id: table.DatabaseId);

            if (_deployService.IsDeployed(database: database)) _deployService.DropDeployedAttribute(attribute: attribute);

            _attributeService.Remove(attribute: attribute);
        }

        public void RenameAttribute(Attribute attribute, string name)
        {
            if (attribute is null)
                throw new ArgumentNullException(nameof(attribute));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (attribute.Name == name) return;

            if (!_attributeValidator.IsValidName(name: name))
                throw new ArgumentException($"Invalid primary key name {name}.");

            Table table = _tableService.GetTableById(tableId: attribute.TableId);

            if (!_attributeValidator.IsUniqueName(table: table, attributeName: name))
                throw new InvalidOperationException($"The attribute {name} is already exists.");

            Database database = _databaseService.GetById(id: table.DatabaseId);

            if (_deployService.IsDeployed(database: database))
            {
                _deployService.RenameDeployedAttribute(attribute: attribute, validAttributeName: name);
            }

            _attributeService.Rename(attribute: attribute, name: name);
        }

        public IEnumerable<Attribute> GetTableAttributes(Table table)
        {
            return _tableService.GetTableAttributes(table: table);
        }

        public Attribute GetAttributeByName(Table table, string name)
        {
            return _attributeService.GetByName(table: table, name: name);
        }

        #endregion

        #region LINK

        public void AddLink(
            Table masterTable,
            Table slaveTable,
            bool isCascadeDelete = false,
            bool isCascadeUpdate = false)
        {
            _linkService.Add(
                masterTable: masterTable,
                slaveTable: slaveTable,
                isCascadeDelete: isCascadeDelete,
                isCascadeUpdate: isCascadeUpdate);
        }

        public IEnumerable<Link> GetDatabaseLinks(Database database)
        {
            return _linkService.GetDatabaseLinks(database: database);
        }

        public void RemoveLink(Link link)
        {
            if (!(_attributeService.GetById(id: link.MasterAttributeId) is PrimaryKey primaryKey))
                throw new NullReferenceException($"The link {link.Id} is not related to any primary key.");

            Table table = _tableService.GetTableById(tableId: primaryKey.TableId);

            Database database = _databaseService.GetById(id: table.DatabaseId);

            if (_deployService.IsDeployed(database: database)) _deployService.DropDeployedLink(link: link);

            _linkService.Remove(link: link);
        }

        #endregion

        #region DEPLOY

        public bool IsDatabaseDeployed(Database database)
        {
            return _deployService.IsDeployed(database: database);
        }

        public bool IsDatabaseDeployable(Database database)
        {
            return _deployService.IsDeployPossible(database: database);
        }

        public void DeployDatabase(Database database)
        {
            _deployService.Deploy(database: database);
        }

        public void UpdateDeployedDatabase(Database database)
        {
            _deployService.UpdateDeployed(database: database);
        }

        public void DropDeployedDatabase(Database database)
        {
            _deployService.DropDeployedDatabase(database: database);
        }

        #endregion
    }
}