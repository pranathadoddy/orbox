using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class CheckoutFormCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<CheckoutFormCode> Lazy =
            new Lazy<CheckoutFormCode>(() => new CheckoutFormCode());

        #endregion

        #region Constructors

        private CheckoutFormCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {                
                new SystemCodeModel<string>(CoreConstant.CheckoutFormOption.SimpleInquiry, TenantResource.OptionCodeCheckoutFormSimpleInquiry),
            };
        }

        #endregion

        #region Properties

        public static CheckoutFormCode Item
        {
            get { return Lazy.Value; }
        }

        public SystemCodeModel<string> SimpleInquiry
        {
            get { return this.GetItem(CoreConstant.CheckoutFormOption.SimpleInquiry); }
        }

        #endregion
    }
}
