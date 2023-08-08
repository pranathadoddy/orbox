using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Linq;

namespace Orderbox.Repository.Common
{
    public class CategoryRepository : BaseTenantRepository<OrderboxContext, ComCategory, CategoryDto, ulong>, ICategoryRepository
    {
        #region Constructor
        public CategoryRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion

        #region Override Methods

        protected override IQueryable<ComCategory> GetKeywordPagedSearchQueryable(IQueryable<ComCategory> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();
            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }

        #endregion
    }
}
