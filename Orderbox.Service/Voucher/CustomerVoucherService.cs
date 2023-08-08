using Framework.Service;
using Orderbox.Dto.Voucher;
using Orderbox.RepositoryContract.Voucher;
using Orderbox.ServiceContract.Voucher;

namespace Orderbox.Service.Voucher
{
    public class CustomerVoucherService : BaseService<CustomerVoucherDto, ulong, ICustomerVoucherRepository>, ICustomerVoucherService
    {
        public CustomerVoucherService(ICustomerVoucherRepository repository) : base(repository)
        {

        }
    }
}
