using Domain.Entities;

namespace Domain.Services.Validators
{
    public interface ILinkValidator
    {
        bool IsUnique(Table masterTable, Table slaveTable);
    }
}