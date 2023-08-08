using System.Security.Claims;
using System.Security.Principal;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Identity
{
    public static class IdentityExtensions
    {
        public static string GetAgencyId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AgencyId");
            return claim?.Value ?? "";
        }

        public static string GetAgentId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AgentId");
            return claim?.Value ?? "";
        }

        public static string GetUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserId");
            return claim?.Value ?? "";
        }

        public static string GetUserName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserName");
            return claim?.Value ?? "";
        }

        public static string GetEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Email");
            return claim?.Value ?? "";
        }

        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            return claim?.Value ?? "";
        }

        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            return claim?.Value ?? "";
        }

        public static string GetFullName(this IIdentity identity)
        {
            var firstNameClaim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            var lastNameClaim = ((ClaimsIdentity)identity).FindFirst("LastName");

            return $"{firstNameClaim?.Value ?? ""} {lastNameClaim?.Value ?? ""}".Trim();
        }

        public static string GetTenantId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("TenantId");
            return claim?.Value ?? "";
        }

        public static string GetTenantShortName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("TenantShortName");
            return claim?.Value ?? "";
        }

        public static string GetLogo(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Logo");
            return claim?.Value ?? "";
        }

        public static string GetRole(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Role);
            return claim?.Value ?? "";
        }
    }
}
