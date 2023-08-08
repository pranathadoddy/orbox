using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;

namespace Orderbox.Repository.Common
{
    public class TenantPushNotificationTokenRepository : BaseTenantRepository<OrderboxContext, ComTenantPushNotificationToken, TenantPushNotificationTokenDto, ulong>, ITenantPushNotificationTokenRepository
    {
        public TenantPushNotificationTokenRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
