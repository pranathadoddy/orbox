using Framework.Application.Controllers;
using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    public class ProductController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IProductImageAssetsManager _productImageAssetsManager;

        public ProductController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            ICategoryService categoryService,
            IProductService productService,
            IProductImageAssetsManager productImageAssetsManager
        ) : base(configuration, hostEnvironment)
        {
            this._categoryService = categoryService;
            this._productService = productService;
            this._productImageAssetsManager = productImageAssetsManager;
        }

        [HttpGet]
        public async Task<ActionResult> Init()
        {
            var tenant = HttpContext.GetTenant();
            var domainNamePart = tenant.Domain.Split(".");
            var tenantShortName = domainNamePart.First();

            var categoryResponse = await this._categoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId = {tenant.Id}"
            });

            if (categoryResponse.IsError())
            {
                return this.GetErrorJson(categoryResponse);
            }

            var categoryList = categoryResponse.DtoCollection.ToList();
            categoryList.Insert(0, new CategoryDto { Id = 0, Name = "All", Description = "All" });
            var totalCategoryItem = categoryResponse.TotalCount;

            var productResponse = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = CoreConstant.Settings.DefaultPageSize,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId = {tenant.Id} and isAvailable = {true}"
            });

            if (productResponse.IsError())
            {
                return this.GetErrorJson(productResponse);
            }

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });

            var productList = productResponse.DtoCollection;
            var totalProductItem = productResponse.TotalCount;

            foreach (var product in productList)
            {
                foreach (var image in product.ProductImages)
                {
                    image.FileName = this._productImageAssetsManager.GetUrl(new GenericRequest<string> { Data = image.FileName }).Data;
                }
            }

            return this.GetSuccessJson(productResponse,
                new
                {
                    ShouldUpdataCache = true, //ToDo: this is to control service worker cache removal (PWA), will be implemented on next release 
                    Categories = categoryList,
                    TotalCategoryItem = totalCategoryItem,
                    Products = productList,
                    TotalProductItem = totalProductItem
                });
        }

        [HttpGet("Tenant/Product/GetByCategory")]
        public async Task<ActionResult> GetByCategoryAsync(ulong cid, int pi, string k)
        {
            var tenant = HttpContext.GetTenant();
            var domainNamePart = tenant.Domain.Split(".");
            var tenantShortName = domainNamePart.First();

            var filters = $"tenantId = {tenant.Id} and isAvailable = {true}";
            if (cid > 0)
            {
                filters = $"{filters} and categoryId = {cid}";
            }

            var response = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = pi - 1,
                PageSize = CoreConstant.Settings.DefaultPageSize,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = k ?? "",
                Filters = filters
            });

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateProductResponse(dto, this._productImageAssetsManager));
            }

            return this.GetPagedSearchGridJson(pi, CoreConstant.Settings.DefaultPageSize, rowJsonData, response);
        }

        [HttpGet("Tenant/Product/Get/{id}")]
        public async Task<ActionResult> GetAsync(ulong id)
        {
            var tenant = HttpContext.GetTenant();
            var domainNamePart = tenant.Domain.Split(".");
            var tenantShortName = domainNamePart.First();

            var response = await this._productService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenant.Id,
                Data = id
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });
            var dto = PopulateProductResponse(response.Data, this._productImageAssetsManager);

            return this.GetSuccessJson(response, dto);
        }

        private object PopulateProductResponse(ProductDto dto, IAssetsManagerBase assetsManagerBase)
        {
            foreach (var image in dto.ProductImages)
            {
                image.FileName = assetsManagerBase.GetUrl(new GenericRequest<string> { Data = image.FileName }).Data;
            }

            return dto;
        }
    }
}
