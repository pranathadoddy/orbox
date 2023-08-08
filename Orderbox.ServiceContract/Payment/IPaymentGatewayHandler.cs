using Framework.ServiceContract.Response;
using Orderbox.ServiceContract.Payment.Request;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Payment
{
    public interface IPaymentGatewayHandler
    {
        Task<GenericResponse<string>> CreatePurchaseAsync(CreatePurchaseRequest request);
    }
}
