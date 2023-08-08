using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class AgencyService : BaseService<AgencyDto, ulong, IAgencyRepository>, IAgencyService
    {
        public AgencyService(IAgencyRepository repository) : base(repository)
        {
            
        }
    }
}