using Framework.Service.FileUpload;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.FileUpload;
using System;

namespace Orderbox.Service.FileUpload
{
    public class ProductImageAssetsManager : AwsS3AssetsManagerBase, IProductImageAssetsManager
    {
        public ProductImageAssetsManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string BaseDirectory => "product";
    }
}
