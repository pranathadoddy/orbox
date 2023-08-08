using AutoMapper;
using Framework.Repository;
using Microsoft.Extensions.Configuration;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Linq;

namespace Orderbox.Repository.Common
{
    public class AgentRepository : BaseRepository<OrderboxContext, ComAgent, AgentDto, ulong>, IAgentRepository
    {
        private readonly IConfiguration _configuration;

        public AgentRepository(
            OrderboxContext context,
            IMapper mapper,
            IConfiguration configuration
        ) : base(context, mapper)
        {
            this._configuration = configuration;
        }

        protected override IQueryable<ComAgent> GetKeywordPagedSearchQueryable(IQueryable<ComAgent> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();

            return entities.Where(item => 
                item.Email.ToLower().Contains(loweredKeyword) ||
                item.Privilege.ToLower().Contains(loweredKeyword));
        }
    }
}
