using System;
using System.Threading.Tasks;
using Framework.Core.Resources;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using Orderbox.Dto.Voucher;
using Orderbox.RepositoryContract.Voucher;
using Orderbox.ServiceContract.Voucher;
using Orderbox.ServiceContract.Voucher.Request;

namespace Orderbox.Service.Voucher
{
    public class VoucherService : BaseService<VoucherDto, ulong, IVoucherRepository>, IVoucherService
    {
        public VoucherService(IVoucherRepository repository) : base(repository)
        {

        }
    }
}
