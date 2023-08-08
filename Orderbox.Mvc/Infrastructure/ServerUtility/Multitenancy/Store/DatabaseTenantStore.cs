using Framework.ServiceContract.Request;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Store
{
    public class DatabaseTenantStore : ITenantStore<Tenant>
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantLogoAssetsManager _tenantLogoAssetsManager;

        public DatabaseTenantStore(ITenantService websiteService, ITenantLogoAssetsManager tenantLogoAssetsManager)
        {
            this._tenantService = websiteService;
            this._tenantLogoAssetsManager = tenantLogoAssetsManager;
        }

        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenantResponse = await this._tenantService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"OrderboxDomain=\"{identifier}\" or CustomDomain=\"{identifier}\""
            });

            if (!tenantResponse.DtoCollection.Any())
            {
                return await Task.FromResult<Tenant>(null);
            }

            var tenantDto = tenantResponse.DtoCollection.First();

            var tenant = new Tenant
            {
                Id = tenantDto.Id,
                Domain = identifier
            };

            this._tenantLogoAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            var tenantLogoResponse = this._tenantLogoAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantDto.Logo });

            tenant.Items.Add("Name", tenantDto.Name);
            tenant.Items.Add("Logo", tenantLogoResponse.Data);
            tenant.Items.Add("Phone", tenantDto.Phone);

            return await Task.FromResult(tenant);
        }
    }
}
