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
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Administrator.Models.Agency;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class AgencyController : BaseController
    {
        private readonly IAgencyService _agencyService;
        public AgencyController(
            IConfiguration configuration,
            IAgencyService dealsAgencyService,
            IWebHostEnvironment webHostEnvironment) : 
            base(configuration, webHostEnvironment)
        {
            this._agencyService = dealsAgencyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"IsDeleted = {false}";
            var response = await this._agencyService.PagedSearchAsync(new PagedSearchRequest
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
                rowJsonData.Add(this.PopulateResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var dto = new AgencyDto
            {
                Name = model.Name,
                Description = model.Description,
            };

            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._agencyService.InsertAsync(new GenericRequest<AgencyDto>
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
                RedirectUrl = $"/Administrator/Agency"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {

            var response = await this._agencyService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"Id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var dto = response.DtoCollection.First();


            var model = new EditModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model)
        {
            var readResponse = await this._agencyService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"Id={model.Id}"
            });

            if (!readResponse.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = readResponse.DtoCollection.First();

            dto.Name = model.Name;
            dto.Description = model.Description;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._agencyService.UpdateAsync(new GenericRequest<AgencyDto>
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

            var readResponse = await this._agencyService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"Id={id}"
            });

            if (!readResponse.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = readResponse.DtoCollection.First();

            dto.IsDeleted = true;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._agencyService.UpdateAsync(new GenericRequest<AgencyDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Administrator/Agency"
            });
        }
        private object PopulateResponse(AgencyDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
                dto.Description
            };
        }

    }
}