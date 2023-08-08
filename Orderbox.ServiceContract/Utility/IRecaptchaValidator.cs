using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Utility
{
    public interface IRecaptchaValidator
    {
        Task<bool> IsValidResponseAsync(string token);
    }
}
