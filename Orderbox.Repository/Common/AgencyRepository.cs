using AutoMapper;
using Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Orderbox.Repository.Common
{
    public class AgencyRepository : BaseRepository<OrderboxContext, ComAgency, AgencyDto, ulong>, IAgencyRepository
    {
        private readonly IConfiguration _configuration;

        public AgencyRepository(
            OrderboxContext context,
            IMapper mapper,
            IConfiguration configuration
        ) : base(context, mapper)
        {
            this._configuration = configuration;
        }

        protected override IQueryable<ComAgency> GetKeywordPagedSearchQueryable(IQueryable<ComAgency> entities, string keyword)
        {
            var loweredKeyword = keyword.ToLower();

            return entities.Where(item => item.Name.ToLower().Contains(loweredKeyword));
        }
    }
}
