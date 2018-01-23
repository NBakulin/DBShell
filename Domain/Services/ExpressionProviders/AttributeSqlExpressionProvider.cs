using System;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Repositories;
using Attribute = Domain.Entities.Attribute.Attribute;
using String = Domain.Entities.Attribute.String;

namespace Domain.Services.ExpressionProviders
{
    public class AttributeSqlExpressionProvider : IAttributeSqlExpressionProvider
    {
        private readonly IRepository<Table> _tableRepository;

        public AttributeSqlExpressionProvider(
            IRepository<Table> tableRepository)
        {
            _tableRepository = tableRepository;
        }

        protected Table GetParentTable(Attribute attribute)
        {
            return
                _tableRepository
                    .All()
                    .Single(t => t.Id == attribute.TableId);
        }

        public string Create(Attribute attribute)
        {
            return
                $"ALTER TABLE {GetParentTable(attribute: attribute).Name} \n" +
                $"\tADD {FullDefinition(attribute)}";
        }

        public string Update(Attribute attribute)
        {
            return
                "EXEC sp_rename \n" +
                $"\'{GetParentTable(attribute: attribute).DeployName}.{attribute.DeployName}\', \'{attribute.Name}\', \'COLUMN\'";

            //$"ALTER TABLE {GetParentTable(attribute: attribute).Name} \n" +
            //$"\tALTER COLUMN {attribute.DeployName} {FullDefinition(attribute)}";
        }

        public string Delele(Attribute attribute)
        {
            return
                $"ALTER TABLE {GetParentTable(attribute: attribute).Name} \n" +
                $"\tDROP COLUMN {attribute.Name}";
        }


        public string FullDefinition(Attribute attribute)
        {
            return
                $"{attribute.Name} {GetTypeString(attribute)} {GetIsNullableString(attribute)} {GetIsPrimaryKeyString(attribute)}";
        }

        protected string GetTypeString(Attribute attribute)
        {
            string defaultTypeString = Enum.GetName(typeof(TSQLType), attribute.SqlType);

            switch (attribute)
            {
                case PrimaryKey _:
                    return defaultTypeString;

                case ForeignKey _:
                    return defaultTypeString;

                case IntegerNumber _:
                    return defaultTypeString;

                case String a when a.Length != null:
                    return defaultTypeString + $"({a.Length})";

                case String _:
                    return defaultTypeString + "(MAX)";

                case RealNumber a when a.BitCapacity != null:
                    return defaultTypeString + $"({a.BitCapacity})";

                case DecimalNumber a when a.Precision != null && a.Scale is null:
                    return defaultTypeString + $"({a.Precision})";

                case DecimalNumber a when a.Precision != null && a.Scale != null:
                    return defaultTypeString + $"({a.Precision},{a.Scale})";

                case DecimalNumber _:
                    return defaultTypeString;

                default:
                    throw new ArgumentException("Unexpected attribute type: " + attribute.GetType());
            }
        }

        protected string GetIsNullableString(Attribute attribute)
        {
            return attribute.IsNullable ? "NULL" : "NOT NULL";
        }

        protected string GetIsPrimaryKeyString(Attribute attribute)
        {
            return attribute.IsPrimaryKey ? "PRIMARY KEY IDENTITY" : string.Empty;
        }
    }
}