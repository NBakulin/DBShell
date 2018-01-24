using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Database : Entity
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; protected set; }

        [Required]
        [MaxLength(64)]
        public string DeployName { get; protected set; }

        [Required]
        [MaxLength(64)]
        protected internal string ServerName { get; protected set; }

        protected internal IList<Table> Tables { get; protected set; }

        protected internal string ConnectionString =>
            $"Data Source={ServerName};Initial Catalog={DeployName};Integrated Security=True";


        protected internal Database() { }

        protected internal Database(string name, string serverName)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DeployName = Name;

            ServerName = serverName ?? throw new ArgumentNullException(nameof(serverName));
        }

        protected internal void Rename(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            {
                if (!IsModified)
                {
                    DeployName = Name;
                }

                Name = name;
            }

            OnModified();
        }
    }
}