using Newtonsoft.Json;

namespace Orderbox.ServiceContract.Utility.Response
{
    public class RecaptchaValidatorResponse
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
