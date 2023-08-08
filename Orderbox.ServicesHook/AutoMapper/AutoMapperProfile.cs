using AutoMapper;

namespace Orderbox.ServicesHook.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            UserEntityMapperSetup.Initialize(this);
            CommonEntityMapperSetup.Initialize(this);
            LocationEntityMapperSetup.Initialize(this);
            TransactionEntityMapperSetup.Initialize(this);
            VoucherEntityMapperSetup.Initialize(this);
        }
    }
}