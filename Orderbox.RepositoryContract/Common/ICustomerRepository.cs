using Framework.RepositoryContract;
using Orderbox.Dto.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.RepositoryContract.Common
{
    public interface ICustomerRepository : IBaseRepository<CustomerDto>
    {
        Task<CustomerDto> ReadByCustomerIdAsync(string Id);
    }
}
