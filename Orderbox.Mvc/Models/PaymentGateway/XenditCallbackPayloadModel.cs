using System;
using System.Text.Json.Serialization;

namespace Orderbox.Mvc.Models.PaymentGateway
{
    public class XenditCallbackPayloadModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("is_high")]
        public bool IsHigh { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("merchant_name")]
        public string MerchantName { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("paid_amount")]
        public decimal PaidAmount { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("paid_at")]
        public DateTime PaidAt { get; set; }

        [JsonPropertyName("payer_email")]
        public string PayerEmail { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("adjusted_received_amount")]
        public decimal AdjustedReceivedAmount { get; set; }

        [JsonPropertyName("fees_paid_amount")]
        public decimal FeesPaidAmount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("payment_channel")]
        public string PaymentChannel { get; set; }

        [JsonPropertyName("payment_destination")]
        public string PaymentDestination { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }
    }
}
