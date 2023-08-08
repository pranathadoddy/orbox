using Orderbox.Core;
using Orderbox.ServiceContract.Payment;
using System.Collections.Generic;

namespace Orderbox.Service.Payment
{
    public class PaymentGatewayManager : IPaymentGatewayManager
    {
        private readonly Dictionary<string, IPaymentGatewayHandler> _handlers;

        public PaymentGatewayManager(IXenditHandler xenditHandler)
        {
            this._handlers = new Dictionary<string, IPaymentGatewayHandler>
            {
                { CoreConstant.PaymentGatewayProvider.Xendit, xenditHandler }
            };
        }

        public Dictionary<string, IPaymentGatewayHandler> Handlers => _handlers;
    }
}
