using System.Linq;
using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services.Validators
{
    public class DatabaseValidator : IDatabaseValidator
    {
        private readonly IRepository<Database> _databaseRepository;
        public DatabaseValidator(
            IRepository<Database> databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public bool IsValidName(string name)
        {
            return new Regex("^[A-Za-zА-Яа-я_][\\wА-Яа-я_]{0,63}$").IsMatch(name);
        }

        public bool IsUniqueName(string name)
        {
            return
                _databaseRepository
                    .All()
                    .All(db => db.Name != name);
        }

        public bool IsValidServerName(string serverName)
        {
            return new Regex("^[\\w_.\\\\]{0,63}$").IsMatch(serverName);
        }
    }
}