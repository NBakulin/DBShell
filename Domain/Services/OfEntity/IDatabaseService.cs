using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.OfEntity
{
    public interface IDatabaseService : IDeployable<Database>
    {
        void Add(string databaseName, string serverName);

        void Remove(Database database);

        Database GetByName(string name);

        Database GetById(int id);

        IEnumerable<Database> GetAll();

        void Rename(Database database, string name);
    }
}