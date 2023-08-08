using System.Linq;
using System.Security.Claims;

namespace Orderbox.Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static bool IsAdministrator(this ClaimsPrincipal principal)
        {
            return principal.Claims.Any(item => item.Type.Equals(ClaimTypes.Role) && item.Value.Equals(CoreConstant.Role.Administrator));
        }
    }
}
