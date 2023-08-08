using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;

namespace Orderbox.Repository.Common
{
    public class ProductStoreRepository : BaseTenantRepository<OrderboxContext, ComProductStore, ProductStoreDto, ulong>, IProductStoreRepository
    {
        public ProductStoreRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task DeleteByProductIdAsync(ulong productId)
        {
            var dbSet = this.Context.Set<ComProductStore>();

            var entities = dbSet.Where(item => item.ProductId == productId);

            foreach (var entity in entities)
            {
                dbSet.Remove(entity);
            }

            await this.Context.SaveChangesAsync();
        }

        public override async Task<PagedSearchResult<ProductStoreDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = 
                this.Context.Set<ComProductStore>()
                    .Include(o => o.Store)
                        .ThenInclude(item => item.City)
                            .ThenInclude(item => item.Country);

            var queryable =
                string.IsNullOrEmpty(parameter.Filters) ?
                    dbSet
                        .AsQueryable() :
                    dbSet
                        .Where(parameter.Filters);

            queryable =
                string.IsNullOrEmpty(parameter.Keyword)
                    ? queryable
                    : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }
    }
}