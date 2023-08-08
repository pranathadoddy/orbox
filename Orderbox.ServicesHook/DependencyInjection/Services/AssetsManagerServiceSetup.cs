using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.FileUpload;
using Orderbox.ServiceContract.FileUpload;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class AssetsManagerServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IPaymentProofAssetsManager, PaymentProofAssetsManager>();
            service.AddScoped<IProductImageAssetsManager, ProductImageAssetsManager>();
            service.AddScoped<ITenantLogoAssetsManager, TenantLogoAssetsManager>();
            service.AddScoped<ITenantWallpaperAssetsManager, TenantWallpaperAssetsManager>();
            service.AddScoped<IAgencyCategoryIconAssetsManager, AgencyCategoryIconAssetsManager>();
        }
    }
}
