using Framework.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orderbox.Core;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using System;

namespace Orderbox.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ILogger<HomeController> logger) :
            base(configuration, hostEnvironment)
        {
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var identity = this.User.Identity;

            if (identity.IsAuthenticated)
            {
                if (identity.GetRole() == CoreConstant.Role.Administrator)
                {
                    return Redirect("/Administrator/Home/Index");
                }
                else if (identity.GetRole() == CoreConstant.Role.User)
                {
                    return Redirect("/User/Home/Index");
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code)
        {
            return View(code);
        }
    }
}
