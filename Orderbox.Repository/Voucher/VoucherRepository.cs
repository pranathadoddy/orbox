using AutoMapper;
using Framework.Repository;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Voucher;
using Orderbox.RepositoryContract.Voucher;
using System.Collections.Generic;

namespace Orderbox.Repository.Voucher
{
    public class VoucherRepository : BaseRepository<OrderboxContext, VchVoucher, VoucherDto, ulong>, IVoucherRepository
    {
        public VoucherRepository(OrderboxContext context, IMapper mapper) : base(context, mapper)
        {

        }

        protected override void DtoToEntity(VoucherDto dto, VchVoucher entity)
        {
            base.DtoToEntity(dto, entity);

            if (dto.CustomerVoucher != null)
            {
                entity.VchCustomerVouchers = new List<VchCustomerVoucher>();
                var customerVoucherEntity = new VchCustomerVoucher();
                this.Mapper.Map(dto.CustomerVoucher, customerVoucherEntity);
                entity.VchCustomerVouchers.Add(customerVoucherEntity);
            }
        }
    }
}
