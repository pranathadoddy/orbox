using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class WalletCode: SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<WalletCode> Lazy =
            new Lazy<WalletCode>(() => new WalletCode());

        #endregion

        #region Constructors

        private WalletCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.Wallet.Ovo, WalletResource.Ovo),
                new SystemCodeModel<string>(CoreConstant.Wallet.Gopay, WalletResource.Gopay),
                new SystemCodeModel<string>(CoreConstant.Wallet.Dana, WalletResource.Dana),
            };
        }

        #endregion

        #region Properties

        public static WalletCode Item
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
