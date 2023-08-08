using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class OrderStatusCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<OrderStatusCode> Lazy =
            new Lazy<OrderStatusCode>(() => new OrderStatusCode());

        #endregion

        #region Constructors

        private OrderStatusCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.OrderStatus.New, OrderResource.Status_New),
                new SystemCodeModel<string>(CoreConstant.OrderStatus.InProgress, OrderResource.Status_InProgress),
                new SystemCodeModel<string>(CoreConstant.OrderStatus.Sent, OrderResource.Status_Sent),
                new SystemCodeModel<string>(CoreConstant.OrderStatus.Finished, OrderResource.Status_Finish),
                new SystemCodeModel<string>(CoreConstant.OrderStatus.Cancelled, OrderResource.Status_Cancelled)
            };
        }

        #endregion

        #region Properties

        public static OrderStatusCode Item
        {
            get { return Lazy.Value; }
        }

        public SystemCodeModel<string> New
        {
            get { return this.GetItem(CoreConstant.OrderStatus.New); }
        }

        public SystemCodeModel<string> InProgress
        {
            get { return this.GetItem(CoreConstant.OrderStatus.InProgress); }
        }

        public SystemCodeModel<string> Sent
        {
            get { return this.GetItem(CoreConstant.OrderStatus.Sent); }
        }

        public SystemCodeModel<string> Finished
        {
            get { return this.GetItem(CoreConstant.OrderStatus.Finished); }
        }

        public SystemCodeModel<string> Cancelled
        {
            get { return this.GetItem(CoreConstant.OrderStatus.Cancelled); }
        }

        #endregion
    }
}
