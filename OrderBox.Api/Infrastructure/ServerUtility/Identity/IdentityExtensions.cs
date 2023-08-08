using Orderbox.Core;
using System.Security.Claims;
using System.Security.Principal;

namespace Orderbox.Api.Infrastructure.ServerUtility.Identity
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value ?? "";
        }

        public static string GetUserName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name);
            return claim?.Value ?? "";
        }

        public static string GetEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Email);
            return claim?.Value ?? "";
        }

        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.GivenName);
            return claim?.Value ?? "";
        }

        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Surname);
            return claim?.Value ?? "";
        }

        public static string GetPicture(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CoreConstant.OrderboxClaimTypes.Picture);
            return claim?.Value ?? "";
        }

        public static string GetIssuer(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CoreConstant.OrderboxClaimTypes.Issuer);
            return claim?.Value ?? "";
        }
    }
}
