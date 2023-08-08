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
using Orderbox.Mvc.Areas.Agent.Models.Country;
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
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public CountryController(
            IConfiguration configuration,
            ICountryService countryService,
            ICityService cityService,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._countryService = countryService;
            this._cityService = cityService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var agencyId = this.User.Identity.GetAgencyId();
            var response = await this._countryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Name",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = model.Keyword,
                Filters = $"AgencyId=\"{agencyId}\""
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateWebsiteResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateModel { };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var agencyId = this.User.Identity.GetAgencyId();

            var dto = new CountryDto
            {
                AgencyId = Convert.ToUInt64(agencyId),
                Name = model.Name
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._countryService.InsertAsync(new GenericRequest<CountryDto>
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
                RedirectUrl = $"/Agent/Country"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var agencyId = this.User.Identity.GetAgencyId();

            var response = await this._countryService.PagedSearchAsync(new PagedSearchRequest
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

            var searchCityResponse = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"countryId={id}"
            });

            var hasCity = searchCityResponse.DtoCollection.Any();

            var model = new EditModel
            {
                Id = dto.Id,
                Name = dto.Name,
                HasCity = hasCity
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

            var agencyId = this.User.Identity.GetAgencyId();

            var response = await this._countryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"AgencyId=\"{agencyId}\" and Id={model.Id}"
            });

            if (!response.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = response.DtoCollection.First();

            dto.Name = model.Name;
            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._countryService.UpdateAsync(new GenericRequest<CountryDto>
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
            var searchCityResponse = await this._cityService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"countryId={id}"
            });

            var hasCity = searchCityResponse.DtoCollection.Any();

            if (hasCity)
            {
                return this.GetErrorJson(CountryResource.Error_CountryHasCity);
            }

            var response = await this._countryService.DeleteAsync(new GenericRequest<ulong>
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
                RedirectUrl = $"/Agent/Country"
            });
        }

        private object PopulateWebsiteResponse(CountryDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
            };
        }
    }
}