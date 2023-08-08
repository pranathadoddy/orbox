using System.Threading.Tasks;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class ProductAgencyCategoryService : BaseService<ProductAgencyCategoryDto, ulong, IProductAgencyCategoryRepository>, IProductAgencyCategoryService
    {
        public ProductAgencyCategoryService(IProductAgencyCategoryRepository repository) : base(repository)
        {
        }

        public async Task<BasicResponse> DeleteByProductIdAsync(GenericRequest<ulong> request)
        {
            await this._repository.DeleteByProductIdAsync(request.Data);
            return new BasicResponse();
        }
    }
}
