using Domain.Entities;

namespace Domain.Services.ExpressionProviders
{
    public sealed class DatabaseSqlExpressionProvider : IDatabaseSqlExpressionProvider
    {
        public string Create(Database database)
        {
            return
                $"CREATE DATABASE {database.Name}";
        }

        public string UpdateName(Database database)
        {
            return
                $"ALTER DATABASE {database.DeployName} \n" +
                $"\tMODIFY NAME = {database.Name}";
        }

        public string Remove(Database database)
        {
            return
                $"ALTER DATABASE {database.Name} \n" +
                "\tSET SINGLE_USER WITH ROLLBACK IMMEDIATE; \n" +
                $"DROP DATABASE {database.Name}";
        }
    }
}