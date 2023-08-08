using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Linq;

namespace Orderbox.Repository.Common
{
    public class PaymentRepository : BaseTenantRepository<OrderboxContext, ComPayment, PaymentDto, ulong>, IPaymentRepository
    {
        #region Constructor

        public PaymentRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion
    }
}
