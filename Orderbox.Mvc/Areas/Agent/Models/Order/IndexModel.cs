namespace Orderbox.Mvc.Areas.Agent.Models.Order
{
    public class IndexModel
    {
        public string Status { get; set; }

        public string MerchantName { get; set; }

        public SideNavigationModel SideNavigation { get; set; }
    }
}