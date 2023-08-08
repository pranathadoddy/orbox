using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class TenantPostNotificationTokenService : BaseTenantService<TenantPushNotificationTokenDto, ulong, ITenantPushNotificationTokenRepository>, ITenantPostNotificationTokenService
    {
        public TenantPostNotificationTokenService(ITenantPushNotificationTokenRepository repository) : base(repository)
        {
        }
    }
}
