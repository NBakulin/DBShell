using System.Data.Entity;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Database = Domain.Entities.Database;

namespace Domain.Repositories
{
    public class DefaultContext : DbContext
    {
        public DefaultContext() : base("DefaultContext")
        {
            Database.Log = System.Console.WriteLine;
        }


        public DbSet<Database> DataBases { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Attribute> Attributes { get; set; }

        public DbSet<Link> Links { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Database>()
                .Property(db => db.ServerName);

            modelBuilder
                .Entity<Table>()
                .Property(t => t.DatabaseId);

            modelBuilder
                .Entity<Attribute>()
                .Property(a => a.TableId);

            modelBuilder
                .Entity<Database>()
                .HasMany(db => db.Tables)
                .WithRequired(t => t.Database)
                .HasForeignKey(t => t.DatabaseId);

            modelBuilder
                .Entity<Table>()
                .HasMany(t => t.Attributes)
                .WithRequired(a => a.Table)
                .HasForeignKey(a => a.TableId);

            modelBuilder
                .Entity<PrimaryKey>()
                .HasMany(pk => pk.Links)
                .WithRequired(l => l.MasterAttribute)
                .HasForeignKey(l => l.MasterAttributeId)
                .WillCascadeOnDelete(false);

            modelBuilder
                .Entity<ForeignKey>()
                .HasMany(fk => fk.Links)
                .WithRequired(l => l.SlaveAttribute)
                .HasForeignKey(l => l.SlaveAttributeId)
                .WillCascadeOnDelete(false);
        }
    }
}