using Domain.Entities;

namespace Domain.Services.Validators
{
    public interface ITableValidator
    {
        bool IsValidName(string tableName);

        bool IsUniqueName(Database database, string tableName);
    }
}