using Framework.RepositoryContract;
using Orderbox.Dto.Common;
using System.Threading.Tasks;

namespace Orderbox.RepositoryContract.Common
{
    public interface ITenantRepository : IBaseRepository<TenantDto>
    {
        Task<TenantDto> ReadByUserIdAsync(object userId);

        Task<bool> IsDomainExistAsync(string subDomain, ulong id);
    }
}
