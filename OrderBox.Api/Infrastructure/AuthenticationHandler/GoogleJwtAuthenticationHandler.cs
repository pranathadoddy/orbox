using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Orderbox.ServiceContract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Orderbox.Core;

namespace Orderbox.Api.Infrastructure.AuthenticationHandler
{
    public class GoogleJwtAuthenticationHandler : AuthenticationHandler<GoogleJwtAuthenticationScheme>
    {
        private readonly ITenantService _tenantService;

        public GoogleJwtAuthenticationHandler(
            IOptionsMonitor<GoogleJwtAuthenticationScheme> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITenantService tenantService) :
            base(options, logger, encoder, clock)
        {
            this._tenantService = tenantService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headerTenantName = "X-Tenant-Name";
            if (!Request.Headers.ContainsKey(headerTenantName))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            StringValues tenantName;
            if (!Request.Headers.TryGetValue(headerTenantName, out tenantName))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            StringValues authorization;
            if (!Request.Headers.TryGetValue("Authorization", out authorization))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            try
            {
                return await ValidateTokenAsync(authorization.ToString().Replace("Bearer ", "").Trim(), tenantName.ToString());
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private async Task<AuthenticateResult> ValidateTokenAsync(string token, string tenantName)
        {
            var tenantResponse =
                await this._tenantService.PagedSearchAsync(new PagedSearchRequest
                {
                    PageIndex = 0,
                    PageSize = 1,
                    OrderByFieldName = "Id",
                    SortOrder = "asc",
                    Keyword = string.Empty,
                    Filters = $"shortname=\"{tenantName}\""
                });
            if (tenantResponse.TotalCount == 0)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var tenantDto = tenantResponse.DtoCollection.First();

            if (string.IsNullOrEmpty(tenantDto.GoogleOauthClientId))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }


            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { tenantDto.GoogleOauthClientId }
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                new Claim(ClaimTypes.Email, payload.Email),
                new Claim(ClaimTypes.Name, payload.Email),
                new Claim(ClaimTypes.GivenName, payload.GivenName),
                new Claim(ClaimTypes.Surname, payload.FamilyName),
                new Claim(CoreConstant.OrderboxClaimTypes.Picture, payload.Picture),
                new Claim(CoreConstant.OrderboxClaimTypes.Issuer, payload.Issuer)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
