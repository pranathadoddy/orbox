using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class PaymentOptionCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<PaymentOptionCode> Lazy =
            new Lazy<PaymentOptionCode>(() => new PaymentOptionCode());

        #endregion

        #region Constructors

        private PaymentOptionCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {                
                new SystemCodeModel<string>(CoreConstant.PaymentOption.Bank, TenantResource.OptionCodeBank),
                new SystemCodeModel<string>(CoreConstant.PaymentOption.Wallet, TenantResource.OptionCodeWallet),
                new SystemCodeModel<string>(CoreConstant.PaymentOption.Cod, TenantResource.OptionCodeCod),
                new SystemCodeModel<string>(CoreConstant.PaymentOption.PaymentGateway, TenantResource.OptionCodePaymentGateway)
            };
        }

        #endregion

        #region Properties

        public static PaymentOptionCode Item
        {
            get { return Lazy.Value; }
        }

        public SystemCodeModel<string> CashOnDelivery
        {
            get { return this.GetItem(CoreConstant.PaymentOption.Cod); }
        }

        public SystemCodeModel<string> Bank
        {
            get { return this.GetItem(CoreConstant.PaymentOption.Bank); }
        }

        public SystemCodeModel<string> Wallet
        {
            get { return this.GetItem(CoreConstant.PaymentOption.Wallet); }
        }

        public SystemCodeModel<string> PaymentGateway
        {
            get { return this.GetItem(CoreConstant.PaymentOption.PaymentGateway); }
        }

        #endregion
    }
}
