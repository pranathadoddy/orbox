using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Orderbox.Core;
using Orderbox.Dto.Authentication;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Identity
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUserDto>
    {
        private readonly IAgentService _agentService;
        private readonly ITenantService _tenantService;
        private readonly ITenantLogoAssetsManager _tenantLogoAssetsManager;
       
        public CustomClaimsPrincipalFactory(
            IAgentService agentService,
            UserManager<ApplicationUserDto> userManager, 
            ITenantService tenantService,
            IOptions<IdentityOptions> optionsAccessor,
            ITenantLogoAssetsManager tenantLogoAssetsManager) : 
            base(userManager, optionsAccessor)
        {
            this._agentService = agentService;
            this._tenantService = tenantService;
            this._tenantLogoAssetsManager = tenantLogoAssetsManager;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUserDto user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await this.UserManager.GetRolesAsync(user);
            var role = roles.First();

            ulong tenantId = 0;
            string tenantShortname = string.Empty;
            ulong agentId = 0;
            ulong agencyId = 0;

            var logo = "";
            if(role == CoreConstant.Role.Administrator)
            {
                logo = user.ProfilePicture ?? "";
            }
            else if(role == CoreConstant.Role.User)
            {
                var tenantResponse = await _tenantService.ReadByUserIdAsync(new GenericRequest<string> { Data = user.Id });
                if(!tenantResponse.IsError())
                {
                    this._tenantLogoAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantResponse.Data.ShortName });
                    var tenantLogoResponse = this._tenantLogoAssetsManager.GetUrl(new GenericRequest<string>
                    {
                        Data = tenantResponse.Data.Logo
                    });
                    logo = tenantLogoResponse.Data;
                    tenantId = tenantResponse.Data.Id;
                    tenantShortname = tenantResponse.Data.ShortName;
                }
            }
            else if(role == CoreConstant.Role.Agent)
            {
                var agentResponse = await _agentService.PagedSearchAsync(new PagedSearchRequest
                {
                    PageIndex = 0,
                    PageSize = 1,
                    OrderByFieldName = "Id",
                    SortOrder = CoreConstant.SortOrder.Descending,
                    Keyword = string.Empty,
                    Filters = $"UserId=\"{user.Id}\""
                });
                if(agentResponse.TotalCount > 0)
                {
                    agentId = agentResponse.DtoCollection.First().Id;
                    agencyId = agentResponse.DtoCollection.First().AgencyId;
                }
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName ?? ""),
                new Claim("Logo", logo),
                new Claim(ClaimTypes.Role, roles.First())
            };

            if(tenantId > 0)
            {
                claims.Add(new Claim("TenantId", tenantId.ToString()));
                claims.Add(new Claim("TenantShortName", tenantShortname));
            }

            if(agentId > 0)
            {
                claims.Add(new Claim("AgentId", agentId.ToString()));
                claims.Add(new Claim("AgencyId", agencyId.ToString()));
            }

            ((ClaimsIdentity)principal.Identity).AddClaims(claims);

            return principal;
        }
    }
}
