using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    public class HomeController : BaseController
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantLogoAssetsManager _tenantLogoAssetsManager;
        private readonly ITenantWallpaperAssetsManager _tenantWallpaperAssetsManager;

        public HomeController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            ITenantService tenantService,
            ITenantLogoAssetsManager tenantLogoAssetsManager,
            ITenantWallpaperAssetsManager tenantWallpaperAssetsManager
        ) : base(configuration, hostEnvironment)
        {
            this._tenantService = tenantService;
            this._tenantLogoAssetsManager = tenantLogoAssetsManager;
            this._tenantWallpaperAssetsManager = tenantWallpaperAssetsManager;
        }

        public IActionResult Index()
        {
            var tenant = HttpContext.GetTenant();

            if (tenant == null) return NotFound();

            return View(tenant);
        }

        public async Task<ActionResult> Info()
        {
            var tenant = HttpContext.GetTenant();
            var domainNamePart = tenant.Domain.Split(".");
            var tenantShortName = domainNamePart.First();

            this._tenantLogoAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });
            this._tenantWallpaperAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });

            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>() { Data = tenant.Id });
            if (tenantResponse.IsError())
            {
                return this.GetErrorJson(tenantResponse);
            }

            tenantResponse.Data.Logo = this._tenantLogoAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantResponse.Data.Logo }).Data;
            tenantResponse.Data.Wallpaper = this._tenantWallpaperAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantResponse.Data.Wallpaper }).Data;

            return this.GetSuccessJson(tenantResponse, tenantResponse.Data);
        }
    }
}
