using Framework.Application.Controllers;
using Framework.Core.Resources;
using Framework.ServiceContract;
using Framework.ServiceContract.FileUpload.Request;
using Framework.ServiceContract.FileUpload.Response;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Dto.Authentication;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Profile;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.Helper;
using Orderbox.ServiceContract.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Profile")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class ProfileController : BaseController
    {
        private readonly ITenantLogoAssetsManager _tenantLogoAssetsManager;
        private readonly ITenantWallpaperAssetsManager _tenantWallpaperAssetsManager;
        private readonly ITenantService _tenantService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUserDto> _userManager;

        public ProfileController(IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ITenantLogoAssetsManager tenantLogoAssetsManager,
            ITenantWallpaperAssetsManager tenantWallpaperAssetsManager,
            ITenantService tenantService,
            IUserService userService,
            UserManager<ApplicationUserDto> userManager) : base(configuration, hostEnvironment)
        {
            this._tenantLogoAssetsManager = tenantLogoAssetsManager;
            this._tenantWallpaperAssetsManager = tenantWallpaperAssetsManager;
            this._tenantService = tenantService;
            this._userService = userService;
            this._userManager = userManager;
        }

        #region Public Methods

        [HttpGet("Index/{tenantId}")]
        public async Task<IActionResult> Index(ulong tenantId)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var readUserResponse = await this._userManager.FindByIdAsync(tenantDto.UserId);

            this._tenantLogoAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            this._tenantWallpaperAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });

            var logoUrlResponse = this._tenantLogoAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantDto.Logo });
            var wallpaperUrlResponse = this._tenantWallpaperAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantDto.Wallpaper });

            var model = new IndexModel
            {
                TenantId = tenantDto.Id,
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Profile"
                },
                LogoUrl = logoUrlResponse.Data,
                WallpaperUrl = wallpaperUrlResponse.Data,
                BusinessName = tenantDto.Name,
                FirstName = readUserResponse.FirstName,
                LastName = readUserResponse.LastName,
                Countries = CountryHelper.GenerateCountrySelecList(tenantDto.CountryCode),
                Address = tenantDto.Address,
                CountryId = tenantDto.CountryCode,
                AreaCode = tenantDto.PhoneAreaCode,
                Phone = tenantDto.Phone,
                AdditionalInformation = tenantDto.AdditionalInformation,
                PaymentDtos = tenantDto.PaymentDtos
            };

            return View(model);
        }

        [HttpPost("UploadLogo")]
        public async Task<IActionResult> UploadLogo(UploadFileModel model)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var tenantDto = tenant as TenantDto;

            var uploadResponse = await this.UploadImageBase64(model.Base64File, model.FileName, tenantDto.ShortName, this._tenantLogoAssetsManager);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            tenantDto.Logo = uploadResponse.ServerFileName;

            var updateTenantResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = tenantDto
            });

            if (updateTenantResponse.IsError())
            {
                return this.GetErrorJson(updateTenantResponse);
            }

            return GetSuccessJson(updateTenantResponse, null);
        }

        [HttpPost("UploadWallpaper")]
        public async Task<IActionResult> UploadWallpaper(UploadFileModel model)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var tenantDto = tenant as TenantDto;

            var uploadResponse = await this.UploadImageBase64(model.Base64File, model.FileName, tenantDto.ShortName, this._tenantWallpaperAssetsManager);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }
            
            tenantDto.Wallpaper = uploadResponse.ServerFileName;

            var updateTenantResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = tenantDto
            });

            if (updateTenantResponse.IsError())
            {
                return this.GetErrorJson(updateTenantResponse);
            }

            return GetSuccessJson(updateTenantResponse, null);
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(IndexModel model)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var tenantDto = tenant as TenantDto;

            var readUserResponse = await this._userManager.FindByIdAsync(tenantDto.UserId);

            var regionInfo = new RegionInfo(model.CountryId);

            tenantDto.Name = model.BusinessName;
            tenantDto.Address = model.Address;
            tenantDto.CountryCode = model.CountryId;
            tenantDto.PhoneAreaCode = model.AreaCode;
            tenantDto.Phone = model.Phone;
            tenantDto.Currency = regionInfo.ISOCurrencySymbol;
            tenantDto.AdditionalInformation = model.AdditionalInformation;

            this.PopulateAuditFieldsOnUpdate(tenantDto);

            var editResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = tenantDto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            var userResponse = await this._userService.ReadByUserIdAsync(new GenericRequest<string> { Data = readUserResponse.Id });
            
            var user = userResponse.Data;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            
            var userEditResponse = await this._userService.UpdateAsync(new UserRequest { User = user });
            
            if (userEditResponse.IsError())
            {
                return this.GetErrorJson(userEditResponse);
            }

            return this.GetSuccessJson(userEditResponse, userEditResponse.Data);
        }

        [HttpPost("UnsetWallpaper/{tenantId}")]
        public async Task<IActionResult> UnsetWallpaper(ulong tenantId)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var tenantDto = tenant as TenantDto;

            tenantDto.Wallpaper = null;

            var updateTenantResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = tenantDto
            });

            if (updateTenantResponse.IsError())
            {
                return this.GetErrorJson(updateTenantResponse);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Agent/Profile/Index/{tenantId}"
            });
        }

        #endregion

        #region Private Methods

        private async Task<FileUploadResponse> UploadImageBase64(string base64PngImage, string fileName, string tenantShortName, IAssetsManagerBase assetsManager)
        {
            var trimmedBase64Image = base64PngImage.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(trimmedBase64Image);
            using (var memoryStream = new MemoryStream(image))
            {
                assetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });
                var uploadResponse = await assetsManager.UploadAsync(new FileUploadRequest
                {
                    FileStream = memoryStream,
                    FileName = fileName,
                    FileSize = (ulong)memoryStream.Length,
                    MimeType = "image/png"
                });

                return uploadResponse;
            }
        }

        #endregion

    }
}