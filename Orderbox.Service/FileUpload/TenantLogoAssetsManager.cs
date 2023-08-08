using Framework.Service.FileUpload;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.FileUpload;

namespace Orderbox.Service.FileUpload
{
    public class TenantLogoAssetsManager : AwsS3AssetsManagerBase, ITenantLogoAssetsManager
    {
        public TenantLogoAssetsManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string BaseDirectory => "logo";
    }
}
