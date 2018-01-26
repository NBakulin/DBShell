using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Database : Entity
    {
        protected internal Database() { }

        protected internal Database(string name, string serverName)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ServerName = serverName ?? throw new ArgumentNullException(nameof(serverName));
        }

        [Required]
        [MaxLength(length: 64)]
        public string Name { get; protected set; }

        [Required]
        [MaxLength(length: 64)]
        protected internal string ServerName { get; protected set; }

        protected internal IList<Table> Tables { get; protected set; }

        protected internal string ConnectionString =>
            $"Data Source={ServerName};Initial Catalog={Name};Integrated Security=True";

        protected internal void Rename(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            OnModified();
        }
    }
}