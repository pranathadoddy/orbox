using Framework.Application.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Orderbox.Mvc.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : BaseController
    {
        public HomeController(
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment) : 
            base(configuration, webHostEnvironment)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}