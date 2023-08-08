using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Core.Resources.Location;
using Orderbox.Dto.Location;
using Orderbox.Mvc.Areas.Agent.Models.City;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Authorize(Roles = "Agent")]
    public class CityController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IStoreService _storeService;

        public CityController(
            IConfiguration configuration,
            ICityService cityService,
            ICountryService countryService,
            IStoreService storeService,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._cityService = cityService;
            this._countryService = countryService;
            this._storeService = storeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var stringAgencyId = this.User.Identity.GetAgencyId();
            var agencyId = ulong.Parse(stringAgencyId);

            var model = new IndexModel
            {
                AgencyId = agencyId,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var stringAgencyId = this.User.Identity.GetAgencyId();
            var agencyId = ulong.Parse(stringAgencyId);

            var response = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Name",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = model.Keyword,
                Filters = $"agencyId={agencyId} and {model.Filters}"
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateWebsiteResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet("Agent/City/Create/{countryId}")]
        public async Task<IActionResult> Create(ulong countryId)
        {
            var stringAgencyId = this.User.Identity.GetAgencyId();
            var agencyId = ulong.Parse(stringAgencyId);

            var response = await this._countryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"agencyId={agencyId} and id={countryId}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var model = new CreateModel
            {
                CountryId = countryId
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

            var stringAgencyId = this.User.Identity.GetAgencyId();
            var agencyId = ulong.Parse(stringAgencyId);

            var dto = new CityDto
            {
                AgencyId = agencyId,
                CountryId = model.CountryId,
                Name = model.Name
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._cityService.InsertAsync(new GenericRequest<CityDto>
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
                RedirectUrl = $"/Agent/Country/Edit/{model.CountryId}"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var agencyId = this.User.Identity.GetAgencyId();

            var response = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"AgencyId=\"{agencyId}\" and Id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var dto = response.DtoCollection.First();

            var searchStoreResponse = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"cityId={id}"
            });

            var hasStore = searchStoreResponse.DtoCollection.Any();

            var model = new EditModel
            {
                Id = dto.Id,
                CountryId = dto.CountryId,
                Name = dto.Name,
                HasStore = hasStore
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model, ulong id)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var agencyId = this.User.Identity.GetAgencyId();

            var response = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"AgencyId=\"{agencyId}\" and Id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = response.DtoCollection.First();

            dto.Name = model.Name;
            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._cityService.UpdateAsync(new GenericRequest<CityDto>
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
            var searchStoreResponse = await this._storeService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"cityId={id}"
            });

            var hasStore = searchStoreResponse.DtoCollection.Any();

            if (hasStore)
            {
                return this.GetErrorJson(CityResource.Error_CityHasStore);
            }

            var response = await this._cityService.DeleteAsync(new GenericRequest<ulong>
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
                RedirectUrl = $"/Agent/Country/Edit/{response.Data.CountryId}"
            });
        }

        private object PopulateWebsiteResponse(CityDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
            };
        }
    }
}