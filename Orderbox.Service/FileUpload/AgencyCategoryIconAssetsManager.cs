using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Service.FileUpload;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.FileUpload;

namespace Orderbox.Service.FileUpload
{
    public class AgencyCategoryIconAssetsManager : AwsS3AssetsManagerBase, IAgencyCategoryIconAssetsManager
    {
        public AgencyCategoryIconAssetsManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string BaseDirectory => "agency-category-icon";
    }
}
