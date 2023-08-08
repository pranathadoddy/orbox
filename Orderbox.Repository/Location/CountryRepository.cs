using AutoMapper;
using Framework.Repository;
using Microsoft.Extensions.Configuration;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Location;
using Orderbox.RepositoryContract.Location;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Orderbox.Repository.Location
{
    public class CountryRepository : BaseRepository<OrderboxContext, LocCountry, CountryDto, ulong>, ICountryRepository
    {
        public CountryRepository(
            OrderboxContext context,
            IMapper mapper
        ) : base(context, mapper)
        { }

        protected override IQueryable<LocCountry> GetKeywordPagedSearchQueryable(IQueryable<LocCountry> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();

            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }
    }
}
