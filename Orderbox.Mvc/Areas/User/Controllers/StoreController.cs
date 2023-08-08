using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Dto.Location;
using Orderbox.Mvc.Areas.User.Models.Store;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class StoreController : BaseController
    {
        private readonly ITenantService _tenantService;
        private readonly ICityService _cityService;
        private readonly IStoreService _storeService;

        public StoreController(
            IConfiguration configuration,
            ICityService cityService,
            IStoreService storeService,
            IWebHostEnvironment webHostEnvironment,
            ITenantService tenantService) : 
            base(configuration, webHostEnvironment)
        {
            this._cityService = cityService;
            this._storeService = storeService;
            this._tenantService = tenantService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var tenantId = this.User.Identity.GetTenantId();

            var response = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Name",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = model.Keyword,
                Filters = $"TenantId=\"{tenantId}\""
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateWebsiteResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateModel 
            {
                Cities = await GetCities()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var dto = new StoreDto
            {
                TenantId = tenantId,
                CityId = model.CityId,
                Name = model.Name,
                Address = model.Address,
                MapUrl = model.MapUrl
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._storeService.InsertAsync(new GenericRequest<StoreDto>
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
                RedirectUrl = $"/User/Store"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());

            var response = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId=\"{tenantId}\" and Id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var dto = response.DtoCollection.First();

            var model = new EditModel
            {
                Id = dto.Id,
                Cities = await GetCities(),
                CityId = dto.CityId,
                Name = dto.Name,
                Address = dto.Address,
                MapUrl = dto.MapUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());

            var response = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId=\"{tenantId}\" and Id={model.Id}"
            });

            if (!response.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = response.DtoCollection.First();

            dto.CityId = model.CityId;
            dto.Name = model.Name;
            dto.Address = model.Address;
            dto.MapUrl = model.MapUrl;
            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._storeService.UpdateAsync(new GenericRequest<StoreDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ulong id)
        {
            var response = await this._storeService.DeleteAsync(new GenericRequest<ulong>
            {
                Data = id
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/User/Store"
            });
        }
        private object PopulateWebsiteResponse(StoreDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
                dto.Address,
                dto.MapUrl,
            };
        }

        private async Task<SelectList> GetCities()
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());

            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            var tenantDto = readTenantResponse.Data;


            var response = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"AgencyId=\"{tenantDto.AgencyId}\""
            });

            return new SelectList(response.DtoCollection, "Id", "Name");
        }
    }
}