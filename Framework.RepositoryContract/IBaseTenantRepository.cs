using System.Threading.Tasks;

namespace Framework.RepositoryContract
{
    public interface IBaseTenantRepository<TDto> : IBaseRepository<TDto>
    {
        Task<TDto> TenantReadAsync(ulong tenantId, object primaryKey);

        Task<TDto> TenantDeleteAsync(ulong tenantId, object primaryKey);
    }

}
