using Orderbox.DataAccess.Application;
using Orderbox.Dto.Authentication;

namespace Orderbox.ServicesHook.AutoMapper
{
    public class UserEntityMapperSetup
    {
        public static void Initialize(AutoMapperProfile profile)
        { 
            profile.CreateMap<AspNetUser, ApplicationUserDto>().ReverseMap();
        }
    }
}