using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core.Resources;
using Framework.ServiceContract;
using Framework.ServiceContract.FileUpload.Request;
using Framework.ServiceContract.FileUpload.Response;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models.AgencyCategory;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Authorize(Roles = "Agent")]
    public class AgencyCategoryController : BaseController
    {
        private readonly IAgencyCategoryService _agencyCategoryService;
        private readonly IAgencyCategoryIconAssetsManager _agencyCategoryIconAssetsManager;
        private readonly IAgencyService _agencyService;

        public AgencyCategoryController(IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            IAgencyCategoryIconAssetsManager agencyCategoryIconAssetsManager,
            IAgencyCategoryService agencyCategoryService,
            IAgencyService agencyService) : base(configuration, hostEnvironment)
        {
            this._agencyCategoryService = agencyCategoryService;
            this._agencyCategoryIconAssetsManager = agencyCategoryIconAssetsManager;
            this._agencyService = agencyService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var agencyId = this.User.Identity.GetAgencyId();

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"AgencyId = {agencyId}";
            var response = await this._agencyCategoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Id",
                SortOrder = CoreConstant.SortOrder.Descending,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(new
                {
                    dto.Id,
                    dto.Name,
                });
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateModel();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var agencyId = ulong.Parse(this.User.Identity.GetAgencyId());
            var agencyShortName = await GetAgencyShortName(agencyId);

            if (string.IsNullOrEmpty(agencyShortName))
            {
                return this.GetErrorJson(new string[]{
                    GeneralResource.Item_NotFound
                });
            }


            var uploadResponse = await this.UploadImageBase64(model.Base64File,
                model.FileName,
                agencyShortName,
                this._agencyCategoryIconAssetsManager);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            var dto = new AgencyCategoryDto
            {
                AgencyId = agencyId,
                Name = model.Name,
                Description = model.Description,
                IsMainCategory = model.IsMainCategory,
                Icon = uploadResponse.ServerFileName
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._agencyCategoryService.InsertAsync(new GenericRequest<AgencyCategoryDto>
            {
                Data = dto
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Agent/AgencyCategory"
            });
        }

        public async Task<IActionResult> Edit(ulong id)
        {
            var agencyId = ulong.Parse(this.User.Identity.GetAgencyId());
            var response = await this._agencyCategoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"AgencyId={agencyId} and id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var dto = response.DtoCollection.First();

            var agencyShortName = await GetAgencyShortName(agencyId);

            if (string.IsNullOrEmpty(agencyShortName))
            {
                return this.GetErrorJson(new string[]{
                    GeneralResource.Item_NotFound
                });
            }

            this._agencyCategoryIconAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = agencyShortName });
            var getIconUrlResponse = this._agencyCategoryIconAssetsManager.GetUrl(new GenericRequest<string> { Data = dto.Icon });
            if (getIconUrlResponse.IsError())
            {
                return NotFound();
            }

            var model = new EditModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsMainCategory = dto.IsMainCategory,
                IconUrl = getIconUrlResponse.Data,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model)
        {
            var response = await this._agencyCategoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"Id={model.Id}"
            });

            if (!response.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = response.DtoCollection.First();

            dto.Name = model.Name;
            dto.Description = model.Description;
            dto.IsMainCategory = model.IsMainCategory;

            if (!string.IsNullOrEmpty(model.Base64File))
            {
                var agencyId = ulong.Parse(this.User.Identity.GetAgencyId());
                var agencyShortName = await GetAgencyShortName(agencyId);

                if (string.IsNullOrEmpty(agencyShortName))
                {
                    return this.GetErrorJson(new string[]{
                    GeneralResource.Item_NotFound
                });
                }


                var uploadResponse = await this.UploadImageBase64(model.Base64File,
                    model.FileName,
                    agencyShortName,
                    this._agencyCategoryIconAssetsManager);

                if (uploadResponse.IsError())
                {
                    return this.GetErrorJson(uploadResponse);
                }

                dto.Icon = uploadResponse.ServerFileName;
            }

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._agencyCategoryService.UpdateAsync(new GenericRequest<AgencyCategoryDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        private async Task<FileUploadResponse> UploadImageBase64(string base64PngImage, string fileName, string agentShortName, IAssetsManagerBase assetsManager)
        {
            var trimmedBase64Image = base64PngImage.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(trimmedBase64Image);
            using (var memoryStream = new MemoryStream(image))
            {
                assetsManager.SetupSubDirectory(new GenericRequest<string> { Data = agentShortName });
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

        private async Task<string> GetAgencyShortName(ulong agencyId)
        {
            var response = await this._agencyService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"Id={agencyId}"
            });

            if (!response.DtoCollection.Any())
            {
                return string.Empty;
            }

            var agencyDto = response.DtoCollection.First();
            return agencyDto.Name.ToLower().Replace(" ", "-");
        }


    }
}
