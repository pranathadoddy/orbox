using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class AgencyCategoryService : BaseService<AgencyCategoryDto, ulong, IAgencyCategoryRepository>, IAgencyCategoryService
    {
        public AgencyCategoryService(IAgencyCategoryRepository repository) : base(repository)
        {
        }
    }
}
