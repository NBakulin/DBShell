using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Domain.Entities;
using Domain.Entities.Link;
using Domain.Repositories;
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

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private static void Main()
        {
            // Check the "App.config" for a presence of properly defined connection string.

            ApiExample();

            PrintAllMetadata();

            Container.Dispose();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void PrintAllMetadata()
        {
            App.GetAllDatabases()
               .ToList()
               .ForEach(db =>
               {
                   Console.WriteLine($"DATABASE: Id = {db.Id} Name = {db.Name} DeployName = {db.Name} IsModified = {db.IsModified}\n");

                   App.GetDatabaseTables(database: db)
                      .ToList()
                      .ForEach(t =>
                      {
                          Console.WriteLine(
                              $"\tTABLE: Id = {t.Id} DatabaseId = {t.DatabaseId} Name = {t.Name} DeployName = {t.Name} IsMod={t.IsModified}\n");

                          App.GetTableAttributes(table: t)
                             .ToList()
                             .ForEach(a =>
                             {
                                 Console.WriteLine(
                                     $"\t\tATTR: Id = {a.Id} TableId = {a.TableId} Name = {a.Name} DeployName = {a.Name} IsModified = {a.IsModified} IsNullable = {a.IsNullable} IsPrimaryKey = {a.IsPrimaryKey} IsIndexed = {a.IsIndexed}");
                                 Console.WriteLine($"\t\t      Type = {a.GetType()}");
                                 Console.WriteLine($"\t\t      Description = {a.Description}");
                                 Console.WriteLine($"\t\t      FormSettings = {a.FormSettings}\n");
                             });
                      });
                   App.GetDatabaseLinks(database: db)
                      .ToList()
                      .ForEach(l => Console.WriteLine(
                                   $"\tLINK: Id = {l.Id} PrimaryKeyID = {l.MasterAttributeId} ForeignKeyID = {l.SlaveAttributeId} IsDeleteCascade = {l.IsDeleteCascade} IsUpdateCascade = {l.IsUpdateCascase} IsModified = {l.IsModified}\n"));
               });
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "FunctionComplexityOverflow")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void ApiExample()
        {
            // API

            #region 1_DATABASE

            #region 1.1 Создание базы данных.

            //    Для содания новой базы данных (включая также сохранение её в метаданных)
            // необходимо вызвать метод CreateDatabase, единственным параметром которого
            // является имя новой БД.
            //    Имя БД должно быть уникальным, начинаться не с цифры и состоять только из
            // русских или английских букв и знака подчеркивания (мин. длина - 1, макс. - 64).
            // Если потенциальное имя не пройдет валидацию, будет вызвано исключение типа
            // ArgumentException.
            //    Если переменная с именем будет содержать ссылку на null, будет вызвано 
            // исключение типа ArgumentNullException.
            try
            {
                App.CreateDatabase("TempDatabase"); // Ок, если ранее бд с таким именем не создавалась
                Console.WriteLine("Новая база данных успешно создана и сохранена.");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при создании базы данных: " + e.Message);
            }

            #endregion

            #region 1.2 Получение экземпляров БД.

            //    Для получения всех сохраненных ранее экземпляров БД необходимо вызвать GetAllDatabases.
            // Если ранее не было создано ни одно экземпляра, будет возвращено значение NULL.

            IEnumerable<Database> allDatabases = App.GetAllDatabases();

            if (allDatabases != null)
                Console.WriteLine("База метаданных содержит ранее созданные экземпляры.");
            else
                Console.WriteLine("База метаданных пуста.");

            //    Кроме того, для того чтобы получить ссылку на экземпляр БД, нужно вызвать GetDatabaseByName или
            // GetDatabaseById с соответствующим параметром. Если соответствующей базы данных не существует,
            // будет возвращено значение NULL.

            const string databaseName = "TempDatabase";

            Database sales = null;

            try
            {
                sales = App.GetDatabaseByName(name: databaseName);
                sales = App.GetDatabaseById(id: sales.Id);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при получении экземпляра БД. " + e.Message);
            }

            if (sales != null)
                Console.WriteLine($"Получена база данных с именем {sales.Name}");
            else
                Console.WriteLine($"Похоже базы данных с именем {databaseName} не существует.");

            #endregion

            #region 1.3 Изменение экземпляров БД

            //    У существующей базы данных можно изменить только имя. Для того, чтобы сделать это,
            // необходимо вызвать RenameDatabase и указать в параметре новое имя. Правила валидации и возможные
            // исключительные ситуации описаны в пункте 1.1.
            try
            {
                Console.WriteLine($"Изменение имени существующей БД {sales?.Name}..");

                App.RenameDatabase(database: sales, databaseName: "TempDatabaseRenamed");

                Console.WriteLine($"Существующая БД успешно переименована. Новое имя: {sales?.Name}");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при изменении существующей БД: " + e.Message);
            }

            #endregion

            #region 1.4 Удаление экземпляров БД

            //   Удалить БД можно с помощью метода RemoveDatabase. Так как каскадное удаление для 
            // экземпляров Link отключено, сначала удаляются все связанные с БД ссылки, а 
            // затем сама база данных.
            if (sales != null)
            {
                int salesId = sales.Id; // Сохранение id для проверки удаления

                Console.WriteLine("Удаление существующей БД..");

                App.RemoveDatabase(database: sales);

                if (App.GetDatabaseById(id: salesId) is null)
                    Console.WriteLine("Существующая база данных была успешно удалена.");
                else
                    Console.WriteLine("Похоже, база данны еще существует...");
            }
            else
                Console.WriteLine("База данных уже была удалена ранее.");

            #endregion

            #endregion

            #region 2_TABLES

            // Тестовая бд Sales для демонстрации работы с таблицами:  
            try
            {
                App.CreateDatabase("Sales");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Database creation failure:" + e.Message);
            }

            sales = App.GetDatabaseByName("Sales");

            #region 2.1 Создание таблиц

            //    Для создания таблиц необходима существующая база данных. Создание новой
            // таблицы осуществляется методом AddTable, у которого в параметрах должны быть
            // существующая БД и имя создаваемой таблицы. Правила валидации и возможные исключения
            // для имен таблиц аналогичны правилам для имен баз данных (пункт 1.1).
            //    При передаче значения null в качестве любого из параметров будет вызвано исключение 
            // ArgumentNullException.
            //    При создании таблицы первичный ключ с именем Id будет создан автоматически.
            try
            {
                App.AddTable(database: sales, name: "Users");
                App.AddTable(database: sales, name: "Customers");
                App.AddTable(database: sales, name: "Orders");
                Console.WriteLine("Таблицы успешно созданы и добавлены.");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при создании таблицы: " + e.Message);
            }

            #endregion

            #region 2.2 Получение экземпляров таблиц

            //    Метод GetDatabaseTables служит для получения всех таблиц,
            // принадлежащих какому-то определенному экземпляру БД, который передается через параметр.
            // Если у базы данных нет таблиц, будет возвращено значение NULL.
            //    При передаче значения null в качестве лэкземпляра БД будет вызвано исключение 
            // ArgumentNullException.
            IEnumerable<Table> databaseTables = null;

            try
            {
                databaseTables = App.GetDatabaseTables(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при получении экземпляров таблиц: " + e.Message);
            }

            if (databaseTables != null)
                foreach (Table table in databaseTables)
                    Console.WriteLine($"База данных {sales.Name} содержит таблицу {table.Name}.");
            else
                Console.WriteLine($"База данных {sales.Name} не содержит таблиц.");

            #endregion

            #region 2.3 Получение таблиц по имени или Id

            //    Для получения таблицы по имени необходимо вызвать метод GetTableByName, указав БД,
            // которой принадлежит таблица, а также её имя. Если талицы с таким именем нет, будет
            // возвращено значение null.
            //    При передаче значения null в качестве любого из параметров будет вызвано исключение 
            // ArgumentNullException.
            Table usersTable = null, customersTable = null, ordersTable = null;
            try
            {
                usersTable = App.GetTableByName(database: sales, name: "Users");
                customersTable = App.GetTableByName(database: sales, name: "Customers");
                ordersTable = App.GetTableByName(database: sales, name: "Orders");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при получении таблиц. " + e.Message);
            }

            if (usersTable != null &&
                customersTable != null &&
                ordersTable != null)
                Console.WriteLine("Таблицы успешно получны по их именам.");
            else
                Console.WriteLine("Ошибка при получении таблиц по именам.");
            //    Для получения таблицы по её Id, необходимо вызвать GetTableById (передав только Id таюлицы).

            if (ordersTable != null) ordersTable = App.GetTableById(id: ordersTable.Id);

            if (ordersTable != null)
                Console.WriteLine("Таблица успешно получена по Id.");
            else
                Console.WriteLine("Ошибка при получении таблицы по Id.");

            #endregion

            #region 2.4 изменение таблиц

            //    У таблиц можно изменить имя. Правила валидации и возможные исключения описаны в 1.1.
            try
            {
                App.RenameTable(table: usersTable, tableName: "SomeName");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка при изменении имени таблицы {usersTable?.Name}. " + e.Message);
            }

            #endregion

            #region 2.5 Удаление таблиц

            if (usersTable != null)
            {
                int tableId = usersTable.Id;

                App.RemoveTable(table: usersTable);

                if (App.GetTableById(id: tableId) is null)
                    Console.WriteLine("Удаление таблицы прошло успешно.");
                else
                    Console.WriteLine("Похоже таблица не была удалена..");
            }
            else
                Console.WriteLine("Таблица уже была удалена ранее.");

            #endregion

            #endregion

            #region 3_ATTRIBUTES

            #region 3.1 Добавление атрибута

            //    Для добавления атрибута нужно вызвать один из методов:
            // AddStringAttribute, AddIntegerAttribute, AddDecimalAttribute, AddRealAttribute
            // в зависимости от необходимого типа. При создании обязательно необходимо указать родительскую таблицу,
            // а также уникальное имя (правила валидации и возможные исключительные ситуации в п. 1.1.).
            // Остальные допустимые параметры, такие как isNullable, необязательны (если их не указать, параметры
            // будут иметь значения по умолчанию).
            try
            {
                App.AddStringAttribute(table: customersTable, name: "FirstName", isNullable: false, length: 50);
                App.AddStringAttribute(table: customersTable, name: "Surname", isNullable: false, length: 70);
                App.AddIntegerAttribute(table: customersTable, name: "Age");

                App.AddStringAttribute(table: ordersTable, name: "ItemName", isNullable: false, length: 25);
                App.AddDecimalAttribute(table: ordersTable, name: "Price", isNullable: false, precision: 8, scale: 2);
                App.AddStringAttribute(table: ordersTable, name: "Other");

                Console.WriteLine("Атрибуты успешно добавлены.");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при добавлении атрибутов. " + e.Message);
            }

            #endregion

            #region 3.2 Получение атрибута

            //    Для получения атрибута необходимо вызвать метод GetAttributeByName,
            // указав родительскую таблицу и имя. Если такого атрибута нет, будет получено значение null.
            _Attribute otherAttribute = null;
            try
            {
                otherAttribute = App.GetAttributeByName(table: ordersTable, name: "Other");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при получении атрибута." + e.Message);
            }

            if (otherAttribute != null)
                Console.WriteLine("Атрибут успешно получен.");
            else
                Console.WriteLine("Ошибка при получении атрибута.");

            //    Помимо этого, можно получить все атрибуты конкретной таблицы, вызвав метод
            // GetTableAttributes с параметром-таблицей.
            IEnumerable<_Attribute> attributes = App.GetTableAttributes(table: ordersTable);

            #endregion

            #region 3.3 Изменение имени атрибута

            //    Правила валидации имени подобны правилам валидации для БД и описаны в 1.1.
            try
            {
                App.RenameAttribute(attribute: otherAttribute, name: "SomeAttribute");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при изменении имени атрибута. " + e.Message);
            }

            #endregion

            #region 3.4 Удаление атрибута

            try
            {
                App.RemoveAttribute(attribute: otherAttribute);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при удалении атрибута. " + e.Message);
            }

            #endregion

            #endregion

            #region 4_LINKS

            #region 4.1 Добавление связи один-ко-многим

            //    Приложение может хранить ссылки (связи) типа один-ко-многим.
            // Для создания ссылки необходимо указать главную (master) и подчиненную (slave)
            // таблицы. При этом в подчиненной таблице будет создан атрибут - внешний ключ.
            try
            {
                App.AddLink(masterTable: customersTable, slaveTable: ordersTable);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при создании связи." + e.Message);
            }

            #endregion

            #region 4.4 Получение ссылки

            Link newLink = App.GetLink(masterTable: customersTable, slaveTable: ordersTable);

            #endregion

            #region 4.2 Получение всех ссылок базы данных

            IEnumerable<Link> salesLinks = null;
            try
            {
                salesLinks = App.GetDatabaseLinks(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при получении ссылок. " + e.Message);
            }

            #endregion

            Link link = salesLinks.FirstOrDefault();

            #region 4.3 Удаление ссылки

            try
            {
                // App.RemoveLink(link);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при удалении ссылки. " + e.Message);
            }

            #endregion

            #endregion

            #region 5_DEPLOY

            #region 5.1 Проверка на возможность развертывания существующей БД

            //    Метод проверяет, что у всех таблиц есть первичный ключ
            try
            {
                bool deployable = App.IsDatabaseDeployable(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при запросе возможности развертывания." + e.Message);
            }

            #endregion

            #region 5.2 Развертывание

            try
            {
                App.DeployDatabase(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при развертывании. " + e.Message);
            }

            #endregion

            #region 5.3 Проверка на наличие уже развернутой аналогичной БД

            try
            {
                bool deployed = App.IsDatabaseDeployed(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("ошбика при проверке возможности развертывания. " + e.Message);
            }

            #endregion

            #region 5.4 Обновление развернутой БД

            // Изменение нескольких параметров в процессе работы с БД..
            App.RenameDatabase(database: sales, databaseName: "SuperSales");
            App.RenameTable(table: customersTable, tableName: "SuperCustomers");
            _Attribute age = App.GetAttributeByName(table: customersTable, name: "Age");
            App.RenameAttribute(attribute: age, name: "SuperAge");

            // Обновление
            try
            {
                App.UpdateDeployedDatabase(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при обновлении развернутой БД. " + e.Message);
            }

            #endregion

            #region 5.5 Удаление развернутой БД

            try
            {
                App.DropDeployedDatabase(database: sales);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Ошибка при удалении развернутой базы данных. " + e.Message);
            }

            #endregion

            #endregion

            App.RemoveDatabase(database: sales);
        }
    }
}