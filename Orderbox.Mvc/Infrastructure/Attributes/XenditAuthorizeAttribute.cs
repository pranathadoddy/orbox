using Microsoft.AspNetCore.Mvc;
using Orderbox.Mvc.Infrastructure.Filter;

namespace Orderbox.Mvc.Infrastructure.Attributes
{
    public class XenditAuthorizeAttribute : TypeFilterAttribute
    {
        public XenditAuthorizeAttribute() : base(typeof(XenditAuthorizeFilter))
        {

        }
    }
}
