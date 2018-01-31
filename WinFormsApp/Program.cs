using System;
using System.Configuration;
using System.Data.Entity;
using System.Windows.Forms;
using Autofac;
using Domain.Repositories;
using Domain.Services;
using Domain.Services.ExpressionProviders;
using Domain.Services.OfEntity;
using Domain.Services.Validators;

namespace Forms
{
    internal class Program
    {
        private static readonly IContainer Container;
        private static readonly App.App App;

        static Program()
        {
            //System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseAlways<DefaultContext>());

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
                .RegisterType<DeploySqlExpressionProvider>()
                .As<IDeploySqlExpressionProvider>();


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
                    parameterValue: ConfigurationManager.ConnectionStrings["DefaultContext"].ConnectionString);

            Container = containerBuilder.Build();

            App = Container.Resolve<App.App>();
        }
        
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(App));

            Container.Dispose();
        }
    }
}