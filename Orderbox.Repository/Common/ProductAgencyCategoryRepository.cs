using System.Collections.Generic;
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
    public class ProductAgencyCategoryRepository : BaseRepository<OrderboxContext, ComProductAgencyCategory, ProductAgencyCategoryDto, ulong>, IProductAgencyCategoryRepository
    {
        #region Constructor
        public ProductAgencyCategoryRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion

        #region Public Methods

        public override async Task<PagedSearchResult<ProductAgencyCategoryDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = this.Context.Set<ComProductAgencyCategory>()
                        .Include(o => o.Product)
                        .ThenInclude(o => o.ComProductImages)
                        .Include(o => o.AgencyCategory);

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

        protected override void DtoToEntity(ProductAgencyCategoryDto dto, ComProductAgencyCategory entity)
        {
            base.DtoToEntity(dto, entity);

            if (dto.Id == 0)
            {
                entity.AgencyCategory = null;
            }
        }

        public async Task DeleteByProductIdAsync(ulong productId)
        {
            var dbSet = this.Context.Set<ComProductAgencyCategory>();

            var entities = dbSet.Where(item=> item.ProductId == productId);

            foreach (var entity in entities)
            {
                dbSet.Remove(entity);
            }
           
            await this.Context.SaveChangesAsync();
        }

        #endregion

        #region Override Methods

        protected override void EntityToDto(ComProductAgencyCategory entity, ProductAgencyCategoryDto dto)
        {
            base.EntityToDto(entity, dto);
            dto.AgencyCategoryName = entity.AgencyCategory.Name;

            if (entity.Product != null)
            {
                var productDto = new ProductDto();
                this.Mapper.Map(entity.Product, productDto);
                if (entity.Product.ComProductImages != null)
                {
                    dto.Product.ProductImages = new List<ProductImageDto>();
                    foreach (var productImageEntity in entity.Product.ComProductImages)
                    {
                        var productImageDto = new ProductImageDto();
                        this.Mapper.Map(productImageEntity, productImageDto);
                        dto.Product.ProductImages.Add(productImageDto);
                    }
                }
            }
        }

        protected override IQueryable<ComProductAgencyCategory> GetKeywordPagedSearchQueryable(IQueryable<ComProductAgencyCategory> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();
            return entities
                .Where(item => item.Product.Name.ToLower().Contains(loweredKeyword));
        }

        #endregion
    }
}
