using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class ProductTypeCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<ProductTypeCode> Lazy =
            new Lazy<ProductTypeCode>(() => new ProductTypeCode());

        #endregion

        #region Constructors

        private ProductTypeCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {                
                new SystemCodeModel<string>(CoreConstant.ProductType.Product, ProductResource.Product),
                new SystemCodeModel<string>(CoreConstant.ProductType.Voucher, ProductResource.Voucher),
            };
        }

        #endregion

        #region Properties

        public static ProductTypeCode Item
        {
            get { return Lazy.Value; }
        }

        public SystemCodeModel<string> Product
        {
            get { return this.GetItem(CoreConstant.ProductType.Product); }
        }

        public SystemCodeModel<string> Voucher
        {
            get { return this.GetItem(CoreConstant.ProductType.Voucher); }
        }

        #endregion
    }
}
