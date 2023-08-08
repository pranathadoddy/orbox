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
using Orderbox.Mvc.Areas.User.Models.Category;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class SubCategoryController : BaseController
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(
            IConfiguration configuration, 
            ISubCategoryService subCategoryService,
            IWebHostEnvironment webHostEnvironment) : 
            base(configuration, webHostEnvironment)
        {
            this._subCategoryService = subCategoryService;
        }

        #region Public Methods

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var tenantId = this.User.Identity.GetTenantId();

            var response = await this._subCategoryService.PagedSearchAsync(new PagedSearchRequest
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
                //rowJsonData.Add(this.PopulateWebsiteResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateModel{};

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            var tenantId = this.User.Identity.GetTenantId();

            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var dto = new SubCategoryDto
            {
                TenantId = Convert.ToUInt64(tenantId),
                Name = model.Name,
                Description = model.Description
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._subCategoryService.InsertAsync(new GenericRequest<SubCategoryDto>
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
                RedirectUrl = $"/User/Category/Index"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var tenantId = this.User.Identity.GetTenantId();

            var response = await this._subCategoryService.PagedSearchAsync(new PagedSearchRequest
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
                Name = dto.Name,
                Description = dto.Description
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

            var tenantId = this.User.Identity.GetTenantId();

            var response = await this._subCategoryService.PagedSearchAsync(new PagedSearchRequest
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

            dto.Name = model.Name;
            dto.Description = model.Description;
            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._subCategoryService.UpdateAsync(new GenericRequest<SubCategoryDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        #endregion

        #region Populate Grid Data

        private object PopulateWebsiteResponse(CategoryDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
            };
        }

        #endregion
    }
}