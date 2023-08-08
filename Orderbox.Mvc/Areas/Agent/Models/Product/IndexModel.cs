﻿namespace Orderbox.Mvc.Areas.Agent.Models.Product
{
    public class IndexModel
    {
        public string MerchantName { get; set; }

        public SideNavigationModel SideNavigation { get; set; }

        public string Currency { get; set; }
        
        public string MerchantRegistrationUrl { get; set; }
    }
}
