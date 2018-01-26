using Domain.Entities;

namespace Domain.Services.ExpressionProviders
{
    public class DeploySqlExpressionProvider : IDeploySqlExpressionProvider
    {
        public string SameNamedDatabaseCount(Database database)
        {
            return
                "SELECT COUNT(*) \n" +
                "FROM sys.databases \n" +
                $"WHERE name = '{database.Name}'";
        }

        public string DatabaseTablesNames(Database database)
        {
            return
                "SELECT TABLE_NAME \n" +
                $"FROM [{database.Name}].[INFORMATION_SCHEMA].[TABLES]";
        }

        public string SameNamedAttributes(Database database, Table table)
        {
            return
                "SELECT COLUMN_NAME \n" +
                $"FROM [{database.Name}].[INFORMATION_SCHEMA].[COLUMNS] \n" +
                $"WHERE TABLE_NAME = '{table.Name}'";
        }
    }
}