using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Domain.Repositories;
using Autofac;
using Domain.Entities;
using Domain.Entities.Link;
using Domain.Services;
using Domain.Services.ExpressionProviders;
using Domain.Services.OfEntity;
using Domain.Services.Validators;
using Database = Domain.Entities.Database;
using _Attribute = Domain.Entities.Attribute.Attribute;

namespace ConsoleApp
{
    internal class Program
    {
        private static readonly IContainer Container;
        private static readonly App.App App;

        private static void Main()
        {
            // Check the "App.config" for a presence of properly defined connection string.

            WriteConnectionStatus();

            ApiExample();

            Container.Dispose();
        }

        private static void WriteConnectionStatus()
        {
            Console.WriteLine($"Connection status: {(App.IsConnectionWorks().ItWorks ? "success" : "failure: " + App.IsConnectionWorks().ErrorMessage)}");
        }

        private static void ApiExample()
        {
            // API

            #region 1_DATABASE

            // 1.1 Create empty database
            try
            {
                App.CreateDatabase("TempDatabase");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Database creation failure:" + e.Message);
            }

            // 1.2 Get database (by its name or id)
            Database
                sales = App.GetDatabaseByName("TempDatabase");
            sales = App.GetDatabaseById(sales.Id);
            // 1.3 Get all databases
            IEnumerable<Database> allDatabases = App.GetAllDatabases();
            // 1.4 Rename database
            App.RenameDatabase(sales, "TempDatabaseRenamed");
            // 1.5 RemoveTable database
            App.RemoveDatabase(sales);

            #endregion

            #region 2_TABLES

            try
            {
                App.CreateDatabase("Sales");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Database creation failure:" + e.Message);
            }

            sales = App.GetDatabaseByName("Sales");

            // 2.1 Create new tables
            App.AddTable(sales, "Users");
            App.AddTable(sales, "Customers");
            App.AddTable(sales, "Orders");
            // 2.2 Get all database tables
            IEnumerable<Table> newDbTables = App.GetDatabaseTables(sales);
            // 2.3 Get table of some database by name
            Table usersTable = App.GetTableByName(sales, "Users");
            Table customersTable = App.GetTableByName(sales, "Customers");
            Table ordersTable = App.GetTableByName(sales, "Orders");
            // 2.4 Rename table 
            App.RenameTable(usersTable, "SomeName");
            // 2.5 RemoveTable table
            App.RemoveTable(usersTable);

            #endregion

            #region 3_ATTRIBUTE

            // 3.1 Add attributes to the tables
            App.AddStringAttribute(customersTable, "FirstName", isNullable: false, length: 50);
            App.AddStringAttribute(customersTable, "Surname", isNullable: false, length: 70);
            App.AddIntegerAttribute(customersTable, "Age");

            App.AddStringAttribute(ordersTable, "ItemName", isNullable: false, length: 25);
            App.AddDecimalAttribute(ordersTable, "Price", isNullable: false, precision: 8, scale: 2);
            App.AddStringAttribute(ordersTable, "Other");
            // 3.2 Get attribute by name
            _Attribute otherAttribute = App.GetAttributeByName(ordersTable, "Other");
            // 3.3 Get all attributes of the table
            IEnumerable<_Attribute> attributes = App.GetTableAttributes(ordersTable);
            // 3.4 Rename attribute
            App.RenameAttribute(otherAttribute, "SomeAttribute");
            // 3.5 RemoveTable attribute
            App.RemoveAttribute(otherAttribute);

            #endregion

            #region 4_LINK

            // 4.1 Add link
            App.AddLink(masterTable: customersTable, slaveTable: ordersTable);
            // 4.2 GetAll links
            IEnumerable<Link> allLinks = App.GetAllLinks(sales);
            Link link = allLinks.FirstOrDefault();
            // 4.3 RemoveTable link
            // App.RemoveLink(link);

            #endregion

            #region 5_DEPLOY

            // 5.1 Check if deployble
            bool deployable = App.IsDatabaseDeployable(sales);
            // 5.2 Deploy database
            App.DeployDatabase(sales);
            // 5.3 Check if deployed
            bool deployed = App.IsDatabaseDeployed(sales);
            // 5.4 Drop database
            App.DropDatabase(sales);

            #endregion

            App.RemoveDatabase(sales);
        }

        static Program()
        {
            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseAlways<DefaultContext>());

            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterGeneric(typeof(EntityFrameworkRepository<>))
                .As(typeof(IRepository<>))
                .SingleInstance();


            containerBuilder
                .RegisterType<DatabaseService>()
                .As<IDatabaseService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<TableService>()
                .As<ITableService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<AttributeService>()
                .As<IAttributeService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<LInkService>()
                .As<ILinkService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<DeployService>()
                .As<IDeployService>()
                .SingleInstance();


            containerBuilder
                .RegisterType<AttributeSqlExpressionProvider>()
                .As<IAttributeSqlExpressionProvider>();

            containerBuilder
                .RegisterType<DatabaseSqlExpressionProvider>()
                .As<IDatabaseSqlExpressionProvider>();

            containerBuilder
                .RegisterType<TableSqlExpressionProvider>()
                .As<ITableSqlExpressionProvider>();

            containerBuilder
                .RegisterType<LinkSqlExpressionProvider>()
                .As<ILinkSqlExpressionProvider>();


            containerBuilder
                .RegisterType<SqlExpressionExecutor>()
                .As<ISqlExpressionExecutor>();

            containerBuilder
                .RegisterInstance(new DefaultContext())
                .As<DbContext>()
                .SingleInstance();


            containerBuilder
                .RegisterType<AttributeValidator>()
                .As<IAttributeValidator>();

            containerBuilder
                .RegisterType<DatabaseValidator>()
                .As<IDatabaseValidator>();

            containerBuilder
                .RegisterType<TableValidator>()
                .As<ITableValidator>();

            containerBuilder
                .RegisterType<LinkValidator>()
                .As<ILinkValidator>();

            containerBuilder
                .RegisterType<App.App>()
                .WithParameter(
                    "connectionString",
                    System.Configuration.ConfigurationManager.ConnectionStrings["DefaultContext"].ConnectionString);

            Container = containerBuilder.Build();

            App = Container.Resolve<App.App>();
        }
    }
}