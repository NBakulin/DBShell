using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Repositories;
using Domain.Services.Validators;
using Attribute = Domain.Entities.Attribute.Attribute;

namespace Domain.Services.OfEntity
{
    public class LInkService : ILinkService
    {
        private readonly IRepository<Link> _linkRepository;
        private readonly IRepository<Attribute> _attributeRepository;
        private readonly IRepository<Table> _tableRepository;
        private readonly ILinkValidator _linkValidator;

        private readonly IAttributeService _attributeService;

        public LInkService(
            IRepository<Link> linkRepository,
            IRepository<Attribute> attributeRepository,
            IRepository<Table> tableRepository,
            ILinkValidator linkValidator,
            IAttributeService attributeService)
        {
            _linkRepository = linkRepository;
            _attributeRepository = attributeRepository;
            _tableRepository = tableRepository;
            _linkValidator = linkValidator;
            _attributeService = attributeService;
        }

        public void Add(
            Table masterTable,
            Table slaveTable,
            bool isCascadeDelete,
            bool isCascadeUpdate)
        {
            if (masterTable is null)
                throw new ArgumentNullException(nameof(masterTable));
            if (slaveTable is null)
                throw new ArgumentNullException(nameof(slaveTable));

            if (!_linkValidator.IsUnique(masterTable:masterTable, slaveTable:slaveTable))
                throw new ArgumentException($"A link between tables {masterTable.Name} and {slaveTable.Name} is already exists.");

            int foreignKeyId = _attributeService.AddForeignKey(masterTable:masterTable, slaveTable:slaveTable);

            ForeignKey foreignKey = _attributeService.GetById(foreignKeyId) as ForeignKey;

            PrimaryKey primaryKey = GetPrimaryKey(table:masterTable);

            Link link = new Link(
                masterAttribute: primaryKey,
                slaveAttribute: foreignKey,
                isDeleteCascade: isCascadeDelete,
                isUpdateCascade: isCascadeUpdate);

            primaryKey.Links.Add(link);

            _linkRepository.Add(link);
        }

        public void Remove(Link link)
        {
            _linkRepository.Remove(link);
        }

        public IEnumerable<Link> GetAll(Database database)
        {
            IQueryable<Table> tables =
                _tableRepository
                    .All()
                    .Where(t => t.DatabaseId == database.Id);

            IQueryable<Attribute> attributes =
                _attributeRepository
                    .All()
                    .Where(a => tables.Any(t => t.Id == a.TableId));

            IQueryable<Link> links =
                _linkRepository
                    .All()
                    .Where(l => attributes.Any(a => a.Id == l.MasterAttributeId));

            return links.ToList();
        }

        public Link GetLink(Table masterTable, Table slaveTable)
        {
            return 
                _linkRepository
                    .All()
                    .SingleOrDefault(l => 
                        l.MasterAttributeId == masterTable.Id && 
                        l.SlaveAttribute.Id == slaveTable.Id);
        }

        public bool IsDeployable(Link entity)
        {
            return true;
        }

        public PrimaryKey GetPrimaryKey(Table table)
        {
            return
                _attributeRepository
                    .All()
                    .Where(a => a.TableId == table.Id)
                    .Where(a => a is PrimaryKey)
                    .OfType<PrimaryKey>()
                    .SingleOrDefault();
        }

        public IEnumerable<ForeignKey> GetForeignKeys(Table table)
        {
            return
                _attributeRepository
                    .All()
                    .Where(a => a.TableId == table.Id)
                    .Where(a => a is ForeignKey)
                    .OfType<ForeignKey>();
        }

        public ForeignKey GetForeignKey(Table masterTable, Table slaveTable)
        {
            return 
                (from slaveForeignKey in GetForeignKeys(slaveTable).ToList()
                 let link = _linkRepository
                        .All()
                        .SingleOrDefault(l => 
                            l.MasterAttributeId == masterTable.Id && 
                            l.SlaveAttribute.Id == slaveForeignKey.Id)
                    where link != null
                    select slaveForeignKey)
                .FirstOrDefault();
        }
    }
}