using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Services;
using Domain.Services.OfEntity;
using Attribute = Domain.Entities.Attribute.Attribute;

namespace App
{
    public class App
    {
        private readonly string _defaultConnectionString;
        private readonly string _serverName;

        private readonly ILinkService _linkService;
        private readonly IAttributeService _attributeService;
        private readonly ITableService _tableService;
        private readonly IDatabaseService _databaseService;
        private readonly IDeployService _deployService;

        public App(
            string connectionString,
            IAttributeService attributeService,
            IDatabaseService databaseService,
            ITableService tableService,
            ILinkService linkService,
            IDeployService deployService)
        {
            _defaultConnectionString = connectionString;
            // sorry for this
            _serverName = new Regex("(?:[Dd]ata\\s+[Ss]ource\\s*=\\s*)(?<server>.*?);")
                .Match(_defaultConnectionString)
                .Groups["server"]
                .Value;

            _attributeService = attributeService;
            _databaseService = databaseService;
            _tableService = tableService;
            _linkService = linkService;
            _deployService = deployService;
        }

        public (bool ItWorks, string ErrorMessage) IsConnectionWorks()
        {
            SqlConnection connection = new SqlConnection(_defaultConnectionString);

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
            return _databaseService.IsDatabaseExist(databaseName);
        }

        public void CreateDatabase(string name)
        {
            _databaseService.Add(name, _serverName);
        }

        public Database GetDatabaseByName(string name)
        {
            return _databaseService.GetByName(name);
        }

        public Database GetDatabaseById(int id)
        {
            return _databaseService.GetById(id);
        }

        public void RemoveDatabase(Database database)
        {
            if (_deployService.IsDeployed(database: database))
            {
                _deployService.DropDeployedDatabase(database: database);
            }

            _databaseService.Remove(database);
        }

        public IEnumerable<Database> GetAllDatabases()
        {
            return _databaseService.GetAll();
        }

        public void RenameDatabase(Database database, string name)
        {
            _databaseService.Rename(database, name);
        }

        #endregion

        #region TABLE

        public void AddTable(Database database, string name)
        {
            _tableService.Add(database, name);
        }

        public IEnumerable<Table> GetDatabaseTables(Database database)
        {
            return _tableService.GetDatabaseTables(database);
        }

        public Table GetTableByName(Database database, string name)
        {
            return _tableService.GetTableByName(database, name);
        }

        public Table GetTableById(int id)
        {
            return _tableService.GetTableById(tableId: id);
        }

        public void RemoveTable(Table table)
        {
            Database database = _databaseService.GetById(table.DatabaseId);

            if (_deployService.IsDeployed(database: database))
            {
                _deployService.DropDeployedTable(table: table);
            }

            _tableService.RemoveTable(table);
        }

        public void RenameTable(Table table, string name)
        {
            _tableService.Rename(table, name);
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
            Table table = _tableService.GetTableById(attribute.TableId);

            Database database = _databaseService.GetById(table.DatabaseId);

            if (_deployService.IsDeployed(database: database))
            {
                _deployService.DropDeployedAttribute(attribute: attribute);
            }

            _attributeService.Remove(attribute);
        }

        public void RenameAttribute(Attribute attribute, string name)
        {
            _attributeService.Rename(attribute, name);
        }

        public IEnumerable<Attribute> GetTableAttributes(Table table)
        {
            return _tableService.GetTableAttributes(table);
        }

        public Attribute GetAttributeByName(Table table, string name)
        {
            return _attributeService.GetByName(table, name);
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
            return _linkService.GetDatabaseLinks(database);
        }

        public void RemoveLink(Link link)
        {
            PrimaryKey primaryKey = _attributeService.GetById(link.MasterAttributeId) as PrimaryKey;

            Table table = _tableService.GetTableById(primaryKey.TableId);

            Database database = _databaseService.GetById(table.DatabaseId);

            if (_deployService.IsDeployed(database: database))
            {
                _deployService.DropDeployedLink(link: link);
            }

            _linkService.Remove(link);
        }

        #endregion

        #region DEPLOY

        public bool IsDatabaseDeployed(Database database)
        {
            return _deployService.IsDeployed(database);
        }

        public bool IsDatabaseDeployable(Database database)
        {
            return _deployService.IsDeployPossible(database);
        }

        public void DeployDatabase(Database database)
        {
            _deployService.Deploy(database);
        }

        public void UpdateDeployedDatabase(Database database)
        {
            _deployService.UpdateDeployed(database: database);
        }

        public void DropDeployedDatabase(Database database)
        {
            _deployService.DropDeployedDatabase(database);
        }

        #endregion
    }
}