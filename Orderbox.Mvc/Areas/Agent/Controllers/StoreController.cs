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
using Orderbox.Dto.Common;
using Orderbox.Dto.Location;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Store;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Location;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Store")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class StoreController : BaseController
    {
        private readonly ICityService _cityService;
        private readonly IStoreService _storeService;

        public StoreController(
            IConfiguration configuration,
            ICityService cityService,
            IStoreService storeService,
            IWebHostEnvironment webHostEnvironment) : 
            base(configuration, webHostEnvironment)
        {
            this._cityService = cityService;
            this._storeService = storeService;
        }

        [HttpGet("Index/{tenantId}")]
        public IActionResult Index(ulong tenantId)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var model = new IndexModel
            {
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Store"
                },
            };
            return View(model);
        }

        [HttpPost("PagedSearchGridJson")]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var response = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Id",
                SortOrder = CoreConstant.SortOrder.Descending,
                Keyword = model.Keyword,
                Filters = model.Filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateWebsiteResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet("Create/{tenantId}")]
        public async Task<IActionResult> Create(ulong tenantId)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var model = new CreateModel 
            {
                TenantId = tenantId,
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Store"
                },
                Cities = await GetCities()
            };
            return View(model);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var dto = new StoreDto
            {
                TenantId = model.TenantId,
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
                RedirectUrl = $"/Agent/Store/Index/{model.TenantId}"
            });
        }

        [HttpGet("Edit/{tenantId}/{id}")]
        public async Task<IActionResult> Edit(ulong tenantId, ulong id)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

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
                TenantId = tenantId,
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Store"
                },
                Id = dto.Id,
                Cities = await GetCities(),
                CityId = dto.CityId,
                Name = dto.Name,
                Address = dto.Address,
                MapUrl = dto.MapUrl
            };

            return View(model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId=\"{model.TenantId}\" and Id={model.Id}"
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

        [HttpPost("Delete/{tenantId}/{id}")]
        public async Task<IActionResult> Delete(ulong tenantId, ulong id)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return NotFound();
            }

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
                RedirectUrl = $"/Agent/Store/Index/{tenantId}"
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
            var agencyId = this.User.Identity.GetAgencyId();

            var response = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"AgencyId=\"{agencyId}\""
            });

            return new SelectList(response.DtoCollection, "Id", "Name");
        }
    }
}