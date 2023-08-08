using Framework.Service.FileUpload;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.FileUpload;

namespace Orderbox.Service.FileUpload
{
    public class TenantWallpaperAssetsManager : AwsS3AssetsManagerBase, ITenantWallpaperAssetsManager
    {
        public TenantWallpaperAssetsManager(IConfiguration configuration) : 
            base(configuration)
        {
        }

        protected override string BaseDirectory => "wallpaper";
    }
}
