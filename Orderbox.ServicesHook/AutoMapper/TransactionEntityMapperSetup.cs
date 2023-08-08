using Orderbox.DataAccess.Application;
using Orderbox.Dto.Transaction;

namespace Orderbox.ServicesHook.AutoMapper
{
    internal class TransactionEntityMapperSetup
    {
        public static void Initialize(AutoMapperProfile profile)
        {
            profile.CreateMap<TrxOrder, OrderDto>().ReverseMap();
            profile.CreateMap<TrxOrderAdditionalCharge, OrderAdditionalChargeDto>().ReverseMap();
            profile.CreateMap<TrxOrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
