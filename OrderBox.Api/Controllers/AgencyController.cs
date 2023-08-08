using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Api.Controllers
{
    [Route("api/agency")]
    [ApiController]
    public class AgencyController : ApiBaseController
    {
        private readonly IAgencyCategoryService _agentCategoryService;
        private readonly IProductAgencyCategoryService _productAgencyCategoryService;
        private readonly ITenantService _tenantService;

        public AgencyController(
            IAgencyCategoryService agentCategoryService,
            IProductAgencyCategoryService productAgencyCategoryService,
            ITenantService tenantService,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment
        ) : base(configuration, hostEnvironment)
        {
            this._agentCategoryService = agentCategoryService;
            this._productAgencyCategoryService = productAgencyCategoryService;
            this._tenantService = tenantService;
        }

        #region Public Methods

        [HttpGet("{id}/categories")]
        public async Task<ActionResult> CategoryPagedSearchGridJson([FromRoute] ulong id, [ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? $"IsMainCategory={true}" : $" and AgencyId = {id} and IsMainCategory={true}");
            var response = await this._agentCategoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Name",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateCategoryResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult> ProductPagedSearchGridJson([FromRoute] ulong id, [FromQuery] ulong categoryId, [ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? $"AgencyId = {id} and AgencyCategoryId={categoryId}" 
                : $" and AgencyId = {id} and AgencyCategoryId={categoryId}");
            var response = await this._productAgencyCategoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateProdictCategoryResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet("{id}/tenants")]
        public async Task<ActionResult> TenantPagedSearchGridJson([FromRoute] ulong id, [ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? $"AgencyId = {id}" : $" and AgencyId = {id}");
            var response = await this._tenantService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Id",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateTenantResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        #endregion

        #region Private Methods

        private object PopulateCategoryResponse(AgencyCategoryDto dto)
        {
            return new
            {
                dto.Id,
                dto.AgencyId,
                dto.Name,
                dto.Description,
                dto.Icon,
            };
        }

        private object PopulateProdictCategoryResponse(ProductAgencyCategoryDto dto)
        {
            var images = new List<Object>();
            if (dto.Product.ProductImages.Count > 0)
            {
                foreach (var image in dto.Product.ProductImages)
                {
                    images.Add(new
                    {
                        image.FileName,
                        image.IsPrimary,
                    });
                }
            }

            var product = new
            {
                dto.Id,
                dto.AgencyId,
                dto.AgencyCategoryId,
                dto.Product.Name,
                dto.Product.Description,
                dto.Product.Price,
                dto.Product.Discount,
                dto.Product.Unit,
                dto.Product.TenantId,
                images
            };

            return product;
        }

        private object PopulateTenantResponse(TenantDto dto)
        {
            return new
            {
                dto.Id,
                dto.AgencyId,
                dto.Name,
                dto.ShortName,
                dto.OrderboxDomain,
                dto.Logo,
                dto.Phone,
                dto.UserId,
            };
        }

        #endregion
    }
}
