using System.Linq;
using Domain.Entities;
using Domain.Entities.Link;
using Domain.Repositories;

namespace Domain.Services.Validators
{
    public class LinkValidator : ILinkValidator
    {
        private readonly IRepository<Link> _linkRepository;
        public LinkValidator(
            IRepository<Link> linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public bool IsUnique(Table masterTable, Table slaveTable)
        {
            return 
                _linkRepository
                    .All()
                    .All(l =>
                        l.MasterAttributeId != masterTable.Id &&
                        l.SlaveAttribute.Id != slaveTable.Id);
        }
    }
}