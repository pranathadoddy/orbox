using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Location;
using Orderbox.RepositoryContract.Location;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Orderbox.Repository.Location
{
    public class StoreRepository : BaseRepository<OrderboxContext, LocStore, StoreDto, ulong>, IStoreRepository
    {
        public StoreRepository(
            OrderboxContext context,
            IMapper mapper
        ) : base(context, mapper)
        { }

        protected override IQueryable<LocStore> GetKeywordPagedSearchQueryable(IQueryable<LocStore> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();

            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }
    }
}
