using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute.Integer;
using Domain.Services.ExpressionProviders;
using Domain.Services.OfEntity;

namespace Domain.Services
{
    // ReSharper disable once InconsistentNaming
    public class CRUD : ICRUD
    {
        private readonly ISqlExpressionExecutor _sqlExecutor;
        private readonly IDatabaseService _databaseService;
        private readonly ICRUDsqlExpressionProvider _crudSqlExpressionProvider;
        private readonly ITableService _tableService;

        public CRUD(
            ISqlExpressionExecutor sqlExecutor,
            IDatabaseService databaseService,
            ICRUDsqlExpressionProvider crudSqlExpressionProvider,
            ITableService tableService)
        {
            _sqlExecutor = sqlExecutor;
            _databaseService = databaseService;
            _crudSqlExpressionProvider = crudSqlExpressionProvider;
            _tableService = tableService;
        }

        public int Insert(Table table, IDictionary<Entities.Attribute.Attribute, string> values)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));
            if (values is null)
                throw new ArgumentNullException(nameof(values));

            IEnumerable<int> tableAttributeIDs =
                _tableService
                    .GetTableAttributes(table: table)
                    .Where(a => !(a is PrimaryKey || a is ForeignKey))
                    .Select(a => a.Id)
                    .OrderBy(id => id);

            IEnumerable<int> keyIDs =
                values
                    .Keys
                    .Select(k => k.Id)
                    .OrderBy(key => key);

            if (!tableAttributeIDs.SequenceEqual(second: keyIDs))
                throw new ArgumentException($"Different list of table \'{table.Name}\' atributes and keys.");

            Database database = _databaseService.GetById(id: table.DatabaseId);

            int rowsAffected = _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _crudSqlExpressionProvider
                    .Insert(table: table, values: values));

            return rowsAffected;
        }

        public int Delete(Table table, int id)
        {
            if (table is null)
                throw new ArgumentNullException(nameof(table));

            Database database = _databaseService.GetById(id: table.DatabaseId);

            int rowsAffected = _sqlExecutor.Execute(
                sqlConnectionString: database.ConnectionString,
                sqlExpression: _crudSqlExpressionProvider
                    .Delete(table: table, id: id));

            return rowsAffected;
        }
    }
}