using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Repositories;
using Attribute = Domain.Entities.Attribute.Attribute;
using String = Domain.Entities.Attribute.String;

namespace Domain.Services.Validators
{
    public class AttributeValidator : IAttributeValidator
    {
        private readonly IRepository<Attribute> _attributeRepository;

        private static readonly Dictionary<Type, HashSet<TSQLType>> Types = new Dictionary<Type, HashSet<TSQLType>>
        {
            {
                typeof(IntegerNumber),
                new HashSet<TSQLType>
                {
                    TSQLType.BIT,
                    TSQLType.SMALLINT,
                    TSQLType.INT,
                    TSQLType.BIGINT,
                    TSQLType.TINYINT
                }
            },
            {
                typeof(PrimaryKey),
                new HashSet<TSQLType>
                {
                    TSQLType.BIT,
                    TSQLType.SMALLINT,
                    TSQLType.INT,
                    TSQLType.BIGINT,
                    TSQLType.TINYINT
                }
            },
            {
                typeof(ForeignKey),
                new HashSet<TSQLType>
                {
                    TSQLType.BIT,
                    TSQLType.SMALLINT,
                    TSQLType.INT,
                    TSQLType.BIGINT,
                    TSQLType.TINYINT
                }
            },
            {
                typeof(DecimalNumber),
                new HashSet<TSQLType>
                {
                    TSQLType.DECIMAL,
                    TSQLType.REAL
                }
            },
            {
                typeof(RealNumber),
                new HashSet<TSQLType>
                {
                    TSQLType.REAL,
                    TSQLType.FLOAT
                }
            },
            {
                typeof(String),
                new HashSet<TSQLType>
                {
                    TSQLType.NCHAR,
                    TSQLType.NVARCHAR
                }
            }
        };

        public AttributeValidator(
            IRepository<Attribute> attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public bool IsValidName(string name)
        {
            return new Regex("^[A-Za-zА-Яа-я_][\\wА-Яа-я_]{0,63}$").IsMatch(name);
        }

        public bool IsUniqueName(Table table, string attributeName)
        {
            return
                _attributeRepository
                    .All()
                    .Where(a => a.TableId == table.Id)
                    .All(a => a.Name != attributeName);
        }

        public bool IsValidType(Type attributeType, TSQLType sqlType)
        {
            return Types[attributeType]?.Contains(sqlType) ?? false;
        }
    }
}