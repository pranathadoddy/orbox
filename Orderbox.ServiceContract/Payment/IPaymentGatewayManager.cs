using System.Collections.Generic;

namespace Orderbox.ServiceContract.Payment
{
    public interface IPaymentGatewayManager
    {
        Dictionary<string, IPaymentGatewayHandler> Handlers { get; }
    }
}
