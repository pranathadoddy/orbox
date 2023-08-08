using Framework.Core.Resources;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;
using System.Threading.Tasks;

namespace Orderbox.Service.Common
{
    public class CustomerService : BaseService<CustomerDto, ulong, ICustomerRepository>, ICustomerService
    {
        public CustomerService(ICustomerRepository repository) : base(repository)
        {
            
        }

        public async Task<GenericResponse<CustomerDto>> ReadByCustomerIdAsync(GenericRequest<string> request)
        {
            var response = new GenericResponse<CustomerDto>();

            response.Data = await this._repository.ReadByCustomerIdAsync(request.Data);

            if (response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }


            return response;
        }
    }
}
