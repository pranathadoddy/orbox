using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class BankCode: SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<BankCode> Lazy =
            new Lazy<BankCode>(() => new BankCode());

        #endregion

        #region Constructors

        private BankCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.Bank.Bca, BankResource.Bca),
                new SystemCodeModel<string>(CoreConstant.Bank.Bni, BankResource.Bni),
                new SystemCodeModel<string>(CoreConstant.Bank.BpdBali, BankResource.BpdBali),
                new SystemCodeModel<string>(CoreConstant.Bank.Bri, BankResource.Bri),
                new SystemCodeModel<string>(CoreConstant.Bank.Btn, BankResource.Btn),
                new SystemCodeModel<string>(CoreConstant.Bank.Btpn, BankResource.Btpn),
                new SystemCodeModel<string>(CoreConstant.Bank.CimbNiaga, BankResource.CimbNiaga),
                new SystemCodeModel<string>(CoreConstant.Bank.Danamon, BankResource.Danamon),
                new SystemCodeModel<string>(CoreConstant.Bank.Mandiri, BankResource.Mandiri),
                new SystemCodeModel<string>(CoreConstant.Bank.Mega, BankResource.Mega),
                new SystemCodeModel<string>(CoreConstant.Bank.Maybank, BankResource.Maybank),
                new SystemCodeModel<string>(CoreConstant.Bank.OcbcNisp, BankResource.OcbcNisp)
            };
        }

        #endregion

        public static BankCode Item
        {
            get { return Lazy.Value; }
        }
    }
}
