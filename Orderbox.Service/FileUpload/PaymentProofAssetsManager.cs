using Framework.Service.FileUpload;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.FileUpload;

namespace Orderbox.Service.FileUpload
{
    public class PaymentProofAssetsManager : AwsS3AssetsManagerBase, IPaymentProofAssetsManager
    {
        public PaymentProofAssetsManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string BaseDirectory => "paymentproof";
    }
}
