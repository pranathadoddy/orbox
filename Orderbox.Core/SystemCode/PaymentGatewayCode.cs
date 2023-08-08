using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class PaymentGatewayCode: SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<PaymentGatewayCode> Lazy =
            new Lazy<PaymentGatewayCode>(() => new PaymentGatewayCode());

        #endregion

        #region Constructors

        private PaymentGatewayCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.PaymentGatewayProvider.Xendit, PaymentGatewayResource.Xendit),
            };
        }

        #endregion

        #region Properties

        public static PaymentGatewayCode Item
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
