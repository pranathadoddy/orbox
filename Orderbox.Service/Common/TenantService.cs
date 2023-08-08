using Framework.Core.Resources;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Common.Request;
using System.Threading.Tasks;

namespace Orderbox.Service.Common
{
    public class TenantService : BaseService<TenantDto, ulong, ITenantRepository>, ITenantService
    {
        public TenantService(ITenantRepository repository) : base(repository)
        {
        }

        public async Task<GenericResponse<bool>> IsDomainExistAsync(IsDomainExistRequest request)
        {
            var response = new GenericResponse<bool>();

            response.Data = await this._repository.IsDomainExistAsync(request.DomainPostfix, request.Id);

            if (response.Data)
            {
                response.AddErrorMessage(Core.Resources.Account.RegistrationResource.DomainExistErrorMessage);
            }

            return response;
        }

        public async Task<GenericResponse<TenantDto>> ReadByUserIdAsync(GenericRequest<string> request)
        {
            var response = new GenericResponse<TenantDto>();

            response.Data = await this._repository.ReadByUserIdAsync(request.Data);

            if (response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }
    }
}
