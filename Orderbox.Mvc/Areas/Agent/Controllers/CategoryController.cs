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
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Category;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Category")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            ICategoryService categoryService) : 
            base(configuration, webHostEnvironment)
        {
            this._categoryService = categoryService;
        }

        [HttpGet("Index/{tenantId}")]
        public IActionResult Index(ulong tenantId)
        {
            object tenant;

            if(!this.HttpContext.Items.TryGetValue("tenant", out tenant))
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
                    ActivePage = "Category"
                }
            };

            return View(model);
        }

        [HttpPost("PagedSearchGridJson")]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var response = await this._categoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Name",
                SortOrder = CoreConstant.SortOrder.Ascending,
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
        public IActionResult Create(ulong tenantId)
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
                    ActivePage = "Category"
                }
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

            var dto = new CategoryDto
            {
                TenantId = model.TenantId,
                Name = model.Name,
                Description = model.Description
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._categoryService.InsertAsync(new GenericRequest<CategoryDto>
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
                RedirectUrl = $"/Agent/Category/Index/{model.TenantId}"
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

            var response = await this._categoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId={tenantId} and id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var dto = response.DtoCollection.First();

            var model = new EditModel
            {
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Category"
                },
                Id = dto.Id,
                TenantId = dto.TenantId,
                Name = dto.Name,
                Description = dto.Description
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

            var response = await this._categoryService.PagedSearchAsync(new PagedSearchRequest
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
            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._categoryService.UpdateAsync(new GenericRequest<CategoryDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        private object PopulateWebsiteResponse(CategoryDto dto)
        {
            return new
            {
                dto.Id,
                dto.Name,
            };
        }
    }
}