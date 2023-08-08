using Orderbox.Core.Resources.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.Report
{
    public class IndexModel
    {
        [Display(Name = "FromDate", ResourceType = typeof(ReportResource))]
        public DateTime DateFrom { get; set; }

        [Editable(false)]
        [Display(Name = "ToDate", ResourceType = typeof(ReportResource))]
        public DateTime DateTo { get; set; }

        public string TenantCurrency { get; set; }
    }
}
