// ReSharper disable InconsistentNaming

namespace Domain.Entities.Attribute
{
    public enum TSQLType
    {
        // Precise num
        BIT,
        TINYINT,
        SMALLINT,
        INT,
        BIGINT,

        DECIMAL,
        NUMERIC,
        SMALLMONEY,
        MONEY,

        // Approximate num
        FLOAT,
        REAL,

        // unicode strings
        NCHAR,
        NVARCHAR

        /*
        // Date and time
        DATE,
        DATETIME2,
        DATETIME,

        // Binary
        BINARY,
        VARBINARY,
        IMAGE
        */
    }
}