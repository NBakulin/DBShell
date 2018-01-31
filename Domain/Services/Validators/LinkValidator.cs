using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Attribute;
using Domain.Entities.Attribute.Integer;
using Domain.Entities.Link;
using Domain.Repositories;

namespace Domain.Services.Validators
{
    public class LinkValidator : ILinkValidator
    {
        private readonly IRepository<Link> _linkRepository;
        private readonly IRepository<Attribute> _attributeRepository;

        public LinkValidator(
            IRepository<Link> linkRepository,
            IRepository<Attribute> attributeRepository)
        {
            _linkRepository = linkRepository;
            _attributeRepository = attributeRepository;
        }

        public bool IsUnique(Table masterTable, Table slaveTable)
        {
            PrimaryKey primaryKey =
                _attributeRepository
                    .All()
                    .OfType<PrimaryKey>()
                    .Single(pk => pk.TableId == masterTable.Id);

            IEnumerable<ForeignKey> foreignKeys =
                _attributeRepository
                    .All()
                    .OfType<ForeignKey>()
                    .Where(fk => fk.TableId == slaveTable.Id);

            return !_linkRepository
                    .All()
                    .Any(l =>
                             l.MasterAttributeId == primaryKey.Id &&
                             foreignKeys.Any(fk => fk.Id == l.SlaveAttributeId));
        }
    }
}