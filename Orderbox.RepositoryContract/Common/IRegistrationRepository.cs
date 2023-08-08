using Orderbox.Dto.Common;
using Framework.RepositoryContract;
using System.Threading.Tasks;

namespace Orderbox.RepositoryContract.Common
{
    public interface IRegistrationRepository : IBaseRepository<RegistrationDto>
    {
        Task<RegistrationDto> ReadByEmailAndVerificationCodeAsync(string email, string code);
    }
}
