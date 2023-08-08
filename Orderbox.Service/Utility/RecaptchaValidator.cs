using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Orderbox.ServiceContract.Utility;
using Orderbox.ServiceContract.Utility.Response;
using System.Net.Http;
using System.Threading.Tasks;

namespace Orderbox.Service.Utility
{
    public class RecaptchaValidator: IRecaptchaValidator
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        
        public RecaptchaValidator(HttpClient client, 
            IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;

        }

        public async Task<bool> IsValidResponseAsync(string token)
        {
            var googleRecaptchaVerifyApi = this._configuration.GetValue<string>("ReCaptcha:GoogleRecaptchaVerifyApi");
            var secretKey = this._configuration.GetValue<string>("ReCaptcha:SecretKey");

            var response = await this._client.GetStringAsync(string.Format(googleRecaptchaVerifyApi, secretKey, token));
            var tokenResponse = JsonConvert.DeserializeObject<RecaptchaValidatorResponse>(response);

            var isValid = tokenResponse.Success && tokenResponse.Score >= 0.5;

            return isValid;
        }
    }
}
