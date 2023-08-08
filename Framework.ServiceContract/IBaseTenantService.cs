using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using System.Threading.Tasks;

namespace Framework.ServiceContract
{
    public interface IBaseTenantService<TDto, TDtoType> : IBaseService<TDto, TDtoType>
    {
        Task<GenericResponse<TDto>> TenantReadAsync(GenericTenantRequest<TDtoType> request);

        Task<GenericResponse<TDto>> TenantDeleteAsync(GenericTenantRequest<TDtoType> request);
    }

}
