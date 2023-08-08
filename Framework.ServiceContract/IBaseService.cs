using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.ServiceContract
{
    public interface IBaseService<TDto, TDtoType>
    {
        #region Public Methods

        Task<GenericPagedSearchResponse<TDto>> PagedSearchAsync(PagedSearchRequest request);

        Task<GenericResponse<TDto>> InsertAsync(GenericRequest<TDto> request);

        Task<GenericResponse<TDto>> ReadAsync(GenericRequest<TDtoType> request);

        Task<GenericResponse<TDto>> UpdateAsync(GenericRequest<TDto> request);

        Task<GenericResponse<TDto>> DeleteAsync(GenericRequest<TDtoType> request);

        Task<GenericResponse<ICollection<TDto>>> BulkInsert(GenericRequest<ICollection<TDto>> request);

        #endregion
    }
}
