using Framework.Application.Presentation;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Order
{
    public class ViewModel
    {
        #region Properties

        public ulong TenantId { get; set; }

        public string MerchantName { get; set; }

        public string TenantShortName { get; set; }

        public ulong Id { get; set; }

        [Required]
        public string Status { get; set; }
        
        public IEnumerable<RadioItem> StatusRadioItems { get; set; }

        public OrderDto Order { get; set; }

        public string PaymentProofUrl { get; set; }

        public string Currency { get; set; }

        public SideNavigationModel SideNavigation { get; set; }

        #endregion
    }
}
