﻿using Domain.Entities;

namespace Domain.Services.ExpressionProviders
{
    public interface ITableSqlExpressionProvider
    {
        string CreateEmpty(Table table);

        string CreateFull(Table table);

        string Rename(Table table, string newValidName);

        string Update(Table table);

        string Remove(Table table);
    }
}