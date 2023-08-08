namespace Orderbox.Mvc.Areas.Agent.Models.Payment
{
    public class PaymentModel
    {
        #region Properties

        public ulong TenantId { get; set; }

        public ulong Id { get; set; }

        public string PaymentOptionCode { get; set; }

        public string ProviderNameBank { get; set; }

        public string ProviderNameWallet { get; set; }

        public string ProviderNamePaymentGateway { get; set; }

        public string PaymentGatewayProviderApiKey { get; set; }

        public string PaymentGatewayProviderWebHookSecret { get; set; }

        public int? PaymentGatewayMinuteDuration { get; set; }

        public string AccountName { get; set; }

        public string AccountNumberBank { get; set; }

        public string AccountNumberWallet { get; set; }

        #endregion
    }
}
