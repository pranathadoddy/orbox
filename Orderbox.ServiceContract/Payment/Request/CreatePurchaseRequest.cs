namespace Orderbox.ServiceContract.Payment.Request
{
    public class CreatePurchaseRequest
    {
        public ulong TenantId { get; set; }

        public ulong OrderId { get; set; }

        public string UserName { get; set; }
    }
}
