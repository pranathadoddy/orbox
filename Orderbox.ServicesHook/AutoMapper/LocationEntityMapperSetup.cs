using Orderbox.DataAccess.Application;
using Orderbox.Dto.Location;

namespace Orderbox.ServicesHook.AutoMapper
{
    public class LocationEntityMapperSetup
    {
        public static void Initialize(AutoMapperProfile profile)
        {
            profile.CreateMap<LocCountry, CountryDto>().ReverseMap();
            profile.CreateMap<LocCity, CityDto>().ReverseMap();
            profile.CreateMap<LocStore, StoreDto>().ReverseMap();
        }
    }
}