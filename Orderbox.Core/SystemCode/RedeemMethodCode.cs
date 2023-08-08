using Framework.Core;
using Orderbox.Core.Resources.Voucher;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class RedeemMethodCode : SystemCodeBase<string> 
    {
        #region Fields

        private static readonly Lazy<RedeemMethodCode> Lazy =
            new Lazy<RedeemMethodCode>(() => new RedeemMethodCode());

        #endregion

        #region Constructors

        public RedeemMethodCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.RedeemMethod.Swipe, VoucherResource.Swipe),
                new SystemCodeModel<string>(CoreConstant.RedeemMethod.Admin, VoucherResource.Admin),
            };
        }

        #endregion

        #region Properties

        public static RedeemMethodCode Item
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
