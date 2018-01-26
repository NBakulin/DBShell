using System.Linq;
using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services.Validators
{
    public class TableValidator : ITableValidator
    {
        private readonly IRepository<Table> _tableRepository;

        public TableValidator(IRepository<Table> tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public bool IsValidName(string tableName)
        {
            return new Regex("^[A-Za-zА-Яа-я_][\\wА-Яа-я_]{0,63}$").IsMatch(input: tableName);
        }

        public bool IsUniqueName(Database database, string tableName)
        {
            return
                _tableRepository
                    .All()
                    .Where(t => t.Id == database.Id)
                    .All(t => t.Name != tableName);
        }
    }
}