using Orderbox.Core.Resources.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Report
{
    public class IndexModel
    {
        public string MerchantName { get; set; }

        public SideNavigationModel SideNavigation { get; set; }

        [Display(Name = "FromDate", ResourceType = typeof(ReportResource))]
        public DateTime DateFrom { get; set; }

        [Editable(false)]
        [Display(Name = "ToDate", ResourceType = typeof(ReportResource))]
        public DateTime DateTo { get; set; }

        public string TenantCurrency { get; set; }
    }
}
