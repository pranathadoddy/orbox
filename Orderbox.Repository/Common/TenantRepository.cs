using AutoMapper;
using Framework.Repository;
using Framework.RepositoryContract;
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
    public class TenantRepository : BaseRepository<OrderboxContext, ComTenant, TenantDto, ulong>, ITenantRepository
    {
        private readonly IConfiguration _configuration;

        public TenantRepository(
            OrderboxContext context, 
            IMapper mapper,
            IConfiguration configuration) : base(context, mapper)
        {
            this._configuration = configuration;
        }

        public override async Task<TenantDto> ReadAsync(object primaryKey)
        {
            var dbSet = this.Context.Set<ComTenant>();
            var entity = await dbSet
                .Include(item => item.ComPayments)
                .Include(item => item.ComTenantPushNotificationTokens)
                .FirstOrDefaultAsync(item => item.Id == (ulong) primaryKey);
            if (entity == null) return null;

            var dto = new TenantDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public override async Task<PagedSearchResult<TenantDto>> PagedSearchAsync(PagedSearchParameter parameter)
        {
            var dbSet = this.Context.Set<ComTenant>();

            var queryable =
                string.IsNullOrEmpty(parameter.Filters)
                    ? dbSet
                        .Include(item => item.ComPayments)
                        .Include(item => item.ComTenantPushNotificationTokens)
                        .AsQueryable()
                    : dbSet
                        .Include(item => item.ComPayments)
                        .Include(item => item.ComTenantPushNotificationTokens)
                        .Where(parameter.Filters);

            queryable =
                string.IsNullOrEmpty(parameter.Keyword)
                    ? queryable
                    : GetKeywordPagedSearchQueryable(queryable, parameter.Keyword);

            return await GetPagedSearchEnumerableAsync(parameter, queryable);
        }

        public async Task<TenantDto> ReadByUserIdAsync(object userId)
        {
            var dbSet = this.Context.Set<ComTenant>();
            var entity =
                await dbSet.FirstOrDefaultAsync(item => item.UserId == (string)userId);
            if (entity == null) return null;

            var dto = new TenantDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public async Task<bool> IsDomainExistAsync(string subDomain, ulong id)
        {
            return await this.Context.ComTenants.AnyAsync(item=> (item.OrderboxDomain.Equals(subDomain) 
            || item.CustomDomain.Equals(subDomain)) && item.Id != id);
        }

        protected override void EntityToDto(ComTenant entity, TenantDto dto)
        {
            base.EntityToDto(entity, dto);

            if (entity.ComPayments != null)
            {
                foreach (var payment in entity.ComPayments)
                {
                    var paymentDto = new PaymentDto();
                    this.Mapper.Map(payment, paymentDto);
                    dto.PaymentDtos.Add(paymentDto);
                }
            }

            if(entity.ComTenantPushNotificationTokens != null && entity.ComTenantPushNotificationTokens.Any())
            {
                var tenantPushNotificationTokenEntity = entity.ComTenantPushNotificationTokens.First();
                dto.TenantPushNotificationTokenDto = new TenantPushNotificationTokenDto();
                this.Mapper.Map(tenantPushNotificationTokenEntity, dto.TenantPushNotificationTokenDto);
            }
        }
    }
}
