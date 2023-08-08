using Framework.Application.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Orderbox.Mvc.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class UserController : BaseController
    {
        public UserController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment) : 
            base(configuration, hostEnvironment)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}