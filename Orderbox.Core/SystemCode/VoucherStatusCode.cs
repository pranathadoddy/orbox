using Framework.Core;
using Orderbox.Core.Resources.Voucher;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class VoucherStatusCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<VoucherStatusCode> Lazy =
            new Lazy<VoucherStatusCode>(() => new VoucherStatusCode());

        #endregion

        #region Constructors

        public VoucherStatusCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.VoucherStatus.Used, VoucherResource.Status_Used),
                new SystemCodeModel<string>(CoreConstant.VoucherStatus.Valid, VoucherResource.Status_Valid)
            };
        }

        #endregion

        #region Properties

        public static VoucherStatusCode Item
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
