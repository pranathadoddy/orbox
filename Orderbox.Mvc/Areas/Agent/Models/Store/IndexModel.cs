namespace Orderbox.Mvc.Areas.Agent.Models.Store
{
    public class IndexModel
    {
        public ulong TenantId { get; set; }

        public string MerchantName { get; set; }
        public SideNavigationModel SideNavigation { get; set; }

        public ulong? AgencyId { get; set; }
       
    }
}
