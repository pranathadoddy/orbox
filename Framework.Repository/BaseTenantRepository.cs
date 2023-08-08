using AutoMapper;
using Framework.Dto;
using Framework.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Framework.Repository
{
    public abstract class BaseTenantRepository<TContext, TEntity, TDto, TDtoType> :
        BaseRepository<TContext, TEntity, TDto, TDtoType>,
        IBaseTenantRepository<TDto>
        where TContext : DbContext, new()
        where TEntity : class, new()
        where TDto : BaseDto<TDtoType>, new()
    {
        protected BaseTenantRepository(TContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async virtual Task<TDto> TenantReadAsync(ulong tenantId, object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.Where($"TenantId = {tenantId} AND Id = {primaryKey}").FirstOrDefaultAsync();
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public async virtual Task<TDto> TenantDeleteAsync(ulong tenantId, object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.Where($"TenantId = {tenantId} AND Id = {primaryKey}").FirstOrDefaultAsync();
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);

            dbSet.Remove(entity);
            await this.Context.SaveChangesAsync();

            return dto;
        }
    }

}
