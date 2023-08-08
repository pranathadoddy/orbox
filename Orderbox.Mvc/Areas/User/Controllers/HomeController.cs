using Framework.Application.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Mvc.Infrastructure.Attributes;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class HomeController : BaseController
    {
        public HomeController(
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment) : 
            base(configuration, webHostEnvironment)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/Order/Index");
        }
    }
}