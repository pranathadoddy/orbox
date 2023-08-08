using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;

namespace Orderbox.Mvc.Areas.Tenant.Models.Order
{
    public class InvoiceModel
    {
        #region Properties

        public ulong Id { get; set; }

        public OrderDto Order { get; set; }

        #endregion
    }
}
