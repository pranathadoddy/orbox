using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.RepositoryContract
{
    public interface IBaseRepository<TDto>
    {
        Task<TDto> InsertAsync(TDto dto);

        Task<TDto> ReadAsync(object primaryKey);

        Task<TDto> UpdateAsync(TDto dto);

        Task<TDto> DeleteAsync(object primaryKey);

        Task<PagedSearchResult<TDto>> PagedSearchAsync(PagedSearchParameter parameter);

        Task<ICollection<TDto>> BulkInsert(ICollection<TDto> dtoCollection);
    }
}
