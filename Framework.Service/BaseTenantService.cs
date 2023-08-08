using Framework.Core.Resources;
using Framework.Dto;
using Framework.RepositoryContract;
using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using System.Threading.Tasks;

namespace Framework.Service
{
    public abstract class BaseTenantService<TDto, TDtoType, TRepository> :
        BaseService<TDto, TDtoType, TRepository>,
        IBaseTenantService<TDto, TDtoType>
        where TDto : AuditableDto<TDtoType>
        where TRepository : IBaseTenantRepository<TDto>
    {
        protected BaseTenantService(TRepository repository) : base(repository)
        {
        }

        public async Task<GenericResponse<TDto>> TenantReadAsync(GenericTenantRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.TenantReadAsync(request.TenantId, request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
                return response;
            }

            response.Data = dto;

            return response;
        }

        public async Task<GenericResponse<TDto>> TenantDeleteAsync(GenericTenantRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.TenantDeleteAsync(request.TenantId, request.Data);
            if (dto == null)
            {
                response.AddErrorMessage(GeneralResource.Item_Delete_NotFound);
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage(GeneralResource.Info_Deleted);

            return response;
        }
    }

}
