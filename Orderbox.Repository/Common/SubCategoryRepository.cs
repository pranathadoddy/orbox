using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Linq;

namespace Orderbox.Repository.Common
{
    public class SubCategoryRepository : BaseTenantRepository<OrderboxContext, ComSubCategory, SubCategoryDto, ulong>, ISubCategoryRepository
    {
        #region Constructor
        public SubCategoryRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion

        #region Override Methods

        protected override IQueryable<ComSubCategory> GetKeywordPagedSearchQueryable(IQueryable<ComSubCategory> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();
            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }

        #endregion
    }
}
