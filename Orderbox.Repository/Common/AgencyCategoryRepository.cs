using System.Linq;
using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;

namespace Orderbox.Repository.Common
{
    public class AgencyCategoryRepository : BaseRepository<OrderboxContext, ComAgencyCategory, AgencyCategoryDto, ulong>, IAgencyCategoryRepository
    {
        #region Constructor
        public AgencyCategoryRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion

        #region Override Methods

        protected override IQueryable<ComAgencyCategory> GetKeywordPagedSearchQueryable(IQueryable<ComAgencyCategory> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();
            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }

        #endregion
    }
}
