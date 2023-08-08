using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class PaymentStatusCode : SystemCodeBase<string>
    {
        private static readonly Lazy<PaymentStatusCode> Lazy =
            new Lazy<PaymentStatusCode>(() => new PaymentStatusCode());

        private PaymentStatusCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.PaymentStatus.Cancelled, OrderResource.PaymentStatus_Cancelled),
                new SystemCodeModel<string>(CoreConstant.PaymentStatus.Expired, OrderResource.PaymentStatus_Expired),
                new SystemCodeModel<string>(CoreConstant.PaymentStatus.Paid, OrderResource.PaymentStatus_Paid),
                new SystemCodeModel<string>(CoreConstant.PaymentStatus.Ready, OrderResource.PaymentStatus_Ready)
            };
        }

        public static PaymentStatusCode Item
        {
            get { return Lazy.Value; }
        }
    }
}