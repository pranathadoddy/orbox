using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class AgentService : BaseService<AgentDto, ulong, IAgentRepository>, IAgentService
    {
        public AgentService(IAgentRepository repository) : base(repository)
        {
            
        }
    }
}