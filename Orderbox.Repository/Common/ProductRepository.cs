using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Orderbox.Repository.Common
{
    public class ProductRepository : BaseTenantRepository<OrderboxContext, ComProduct, ProductDto, ulong>, IProductRepository
    {
        #region Constructor

        public ProductRepository(
            OrderboxContext context, 
            IMapper mapper) : base(context, mapper)
        {

        }

        #endregion

        #region Override Methods

        public override async Task<ProductDto> TenantReadAsync(ulong tenantId, object primaryKey)
        {
            var dbSet = this.Context.Set<ComProduct>();
            var entity = await dbSet
                .Include(item => item.Tenant)
                .Include(item => item.ComProductImages)
                .FirstOrDefaultAsync(item => item.TenantId == tenantId && item.Id == (ulong) primaryKey);
            if (entity == null) return null;

            var dto = new ProductDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public override async Task<PagedSearchResult<ProductDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = this.Context.Set<ComProduct>();

            var queryable =
                string.IsNullOrEmpty(parameter.Filters)
                    ? dbSet
                        .Include(item => item.Tenant)
                        .Include(item => item.ComProductImages)
                        .AsQueryable()
                    : dbSet
                        .Include(item => item.Tenant)
                        .Include(item => item.ComProductImages)
                        .Where(parameter.Filters);

            queryable =
                string.IsNullOrEmpty(parameter.Keyword)
                    ? queryable
                    : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }

        protected override IQueryable<ComProduct> GetKeywordPagedSearchQueryable(IQueryable<ComProduct> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();
            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }

        protected override void EntityToDto(ComProduct entity, ProductDto dto)
        {
            base.EntityToDto(entity, dto);

            if (entity.ComProductImages != null)
            {
                dto.ProductImages = new List<ProductImageDto>();
                foreach (var productImageEntity in entity.ComProductImages)
                {
                    var productImageDto = new ProductImageDto();
                    this.Mapper.Map(productImageEntity, productImageDto);
                    dto.ProductImages.Add(productImageDto);
                }
            }
        }

        #endregion
    }
}
