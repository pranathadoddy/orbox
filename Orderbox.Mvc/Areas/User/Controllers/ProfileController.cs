using Framework.Application.Controllers;
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
using Orderbox.Mvc.Areas.User.Models.Profile;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.Helper;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class ProfileController : BaseController
    {
        private readonly ITenantLogoAssetsManager _tenantLogoAssetsManager;
        private readonly ITenantWallpaperAssetsManager _tenantWallpaperAssetsManager;
        private readonly ITenantService _tenantService;
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUserDto> _signInManager;

        public ProfileController(IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ITenantLogoAssetsManager tenantLogoAssetsManager,
            ITenantWallpaperAssetsManager tenantWallpaperAssetsManager,
            ITenantService tenantService,
            IUserService userService,
            SignInManager<ApplicationUserDto> signInManager) : base(configuration, hostEnvironment)
        {
            this._tenantLogoAssetsManager = tenantLogoAssetsManager;
            this._tenantWallpaperAssetsManager = tenantWallpaperAssetsManager;
            this._tenantService = tenantService;
            this._userService = userService;
            this._signInManager = signInManager;
        }

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());
            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            var user = this.User.Identity;
            var tenantDto = readTenantResponse.Data;

            this._tenantLogoAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            this._tenantWallpaperAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });

            var logoUrlResponse = this._tenantLogoAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantDto.Logo });
            var wallpaperUrlResponse = this._tenantWallpaperAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantDto.Wallpaper });

            var model = new IndexModel
            {
                LogoUrl = logoUrlResponse.Data,
                WallpaperUrl = wallpaperUrlResponse.Data,
                BusinessName = tenantDto.Name,
                FirstName = user.GetFirstName(),
                LastName = user.GetLastName(),
                Countries = CountryHelper.GenerateCountrySelecList(tenantDto.CountryCode),
                Address = tenantDto.Address,
                CountryId = tenantDto.CountryCode,
                AreaCode = tenantDto.PhoneAreaCode,
                Phone = tenantDto.Phone,
                AdditionalInformation = tenantDto.AdditionalInformation,
                PaymentDtos = tenantDto.PaymentDtos
            };

            return this.PartialView("_Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadLogo(UploadFileModel model)
        {
            var uploadResponse = await this.UploadImageBase64(model.Base64File, model.FileName, this._tenantLogoAssetsManager);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());
            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            if (readTenantResponse.IsError())
            {
                return this.GetErrorJson(readTenantResponse);
            }

            var dto = readTenantResponse.Data;
            dto.Logo = uploadResponse.ServerFileName;

            var updateTenantResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = dto
            });

            if (updateTenantResponse.IsError())
            {
                return this.GetErrorJson(updateTenantResponse);
            }

            return GetSuccessJson(updateTenantResponse, null);
        }

        [HttpPost]
        public async Task<IActionResult> UploadWallpaper(UploadFileModel model)
        {
            var uploadResponse = await this.UploadImageBase64(model.Base64File, model.FileName, this._tenantWallpaperAssetsManager);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());
            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            if (readTenantResponse.IsError())
            {
                return this.GetErrorJson(readTenantResponse);
            }

            var dto = readTenantResponse.Data;
            dto.Wallpaper = uploadResponse.ServerFileName;

            var updateTenantResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = dto
            });

            if (updateTenantResponse.IsError())
            {
                return this.GetErrorJson(updateTenantResponse);
            }

            return GetSuccessJson(updateTenantResponse, null);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IndexModel model)
        {
            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());
            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            if (readTenantResponse.IsError()) return this.GetErrorJson(readTenantResponse);

            var tenantDto = readTenantResponse.Data;

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

            var userId = this.User.Identity.GetUserId();
            var userResponse = await this._userService.ReadByUserIdAsync(new GenericRequest<string> { Data = userId });
            var user = userResponse.Data;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var userEditResponse = await this._userService.UpdateAsync(new UserRequest { User = user });
            if (userEditResponse.IsError())
            {
                return this.GetErrorJson(userEditResponse);
            }

            await this._signInManager.RefreshSignInAsync(user);

            return this.GetSuccessJson(userEditResponse, userEditResponse.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UnsetWallpaper()
        {
            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());
            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            if (readTenantResponse.IsError())
            {
                return this.GetErrorJson(readTenantResponse);
            }

            var dto = readTenantResponse.Data;
            dto.Wallpaper = null;

            var updateTenantResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = dto
            });

            if (updateTenantResponse.IsError())
            {
                return this.GetErrorJson(updateTenantResponse);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/User/Profile/Index"
            });
        }

        #endregion

        #region Private Methods

        private async Task<FileUploadResponse> UploadImageBase64(string base64PngImage, string fileName, IAssetsManagerBase assetsManager)
        {
            var trimmedBase64Image = base64PngImage.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(trimmedBase64Image);
            using (var memoryStream = new MemoryStream(image))
            {
                var tenantShortName = this.User.Identity.GetTenantShortName();

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