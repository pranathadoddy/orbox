using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class PaymentService : BaseTenantService<PaymentDto, ulong, IPaymentRepository>, IPaymentService
    {
        #region Constructor

        public PaymentService(IPaymentRepository repository) : base(repository)
        {
        }

        #endregion
    }
}
