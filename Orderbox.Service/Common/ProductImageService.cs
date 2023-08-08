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
    public class ProductImageService : BaseTenantService<ProductImageDto, ulong, IProductImageRepository>, IProductImageService
    {
        public ProductImageService(IProductImageRepository repository) : base(repository)
        {
        }

        public async Task<GenericResponse<ProductImageDto>> SetPrimaryAsync(GenericRequest<ProductImageDto> request)
        {
            var response = new GenericResponse<ProductImageDto>();

            if (response.IsError()) return response;

            var dto = await _repository.SetPrimaryAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_Update_NotFound);
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage(GeneralResource.Info_Saved);

            return response;
        }
    }
}
