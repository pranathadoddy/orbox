using AutoMapper;
using Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Threading.Tasks;

namespace Orderbox.Repository.Common
{
    public class ProductImageRepository: BaseTenantRepository<OrderboxContext, ComProductImage, ProductImageDto, ulong>, IProductImageRepository
    {
        public ProductImageRepository(
            OrderboxContext context,
            IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<ProductImageDto> SetPrimaryAsync(ProductImageDto dto)
        {
            var dbSet = this.Context.Set<ComProductImage>();
            var entity = await dbSet.FirstOrDefaultAsync(item => item.Id == dto.Id);
            if (entity == null) return null;

            this.Mapper.Map(dto, entity);

            var productId = entity.ProductId;
            var previousPrimaryEntity = await dbSet.FirstOrDefaultAsync(item => item.ProductId == productId && item.IsPrimary);
            previousPrimaryEntity.IsPrimary = false;
            previousPrimaryEntity.CreatedBy = dto.LastModifiedBy;
            previousPrimaryEntity.CreatedDateTime = dto.LastModifiedDateTime;

            dbSet.Update(entity);
            dbSet.Update(previousPrimaryEntity);
            await this.Context.SaveChangesAsync();

            return dto;
        }
    }
}
