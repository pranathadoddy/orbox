using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core.Resources;
using Framework.ServiceContract;
using Framework.ServiceContract.FileUpload.Request;
using Framework.ServiceContract.FileUpload.Response;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Core.Resources.Common;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.User.Models.Product;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class ProductController : BaseController
    {
        private readonly ITenantService _tenantService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IProductImageAssetsManager _productImageAssetsManager;

        public ProductController(
            IConfiguration configuration,
            ITenantService tenantService,
            IProductService productService,
            IProductImageService productImageService,
            ICategoryService categoryService,
            IProductImageAssetsManager productImageAssetsManager,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._tenantService = tenantService;
            this._productService = productService;
            this._productImageService = productImageService;
            this._categoryService = categoryService;
            this._productImageAssetsManager = productImageAssetsManager;
        }

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var response = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = tenantId });
            var tenantDto = response.Data;

            var model = new IndexModel
            {
                Currency = tenantDto.Currency
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var tenantId = this.User.Identity.GetTenantId();
            var tenantShortName = this.User.Identity.GetTenantShortName();
            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"tenantId = {tenantId}";

            var response = await this._productService.PagedSearchAsync(new PagedSearchRequest
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
                rowJsonData.Add(this.PopulateProductResponse(dto, tenantShortName, this._productImageAssetsManager));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateModel
            {
                IsAvailable = true,
                Unit = CoreConstant.Product.DefaultUnit,
                Discount = 0,
                Categories = await GetCategories()
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

            var tenantId = this.User.Identity.GetTenantId();

            var dto = new ProductDto
            {
                TenantId = Convert.ToUInt64(tenantId),
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Discount = model.Discount == null ? 0 : model.Discount.Value,
                Unit = model.Unit,
                IsAvailable = model.IsAvailable
            };

            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._productService.InsertAsync(new GenericRequest<ProductDto>
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
                RedirectUrl = $"/User/Product/Index"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var tenantId = this.User.Identity.GetTenantId();
            var tenantShortName = this.User.Identity.GetTenantShortName();

            var response = await this._productService.PagedSearchAsync(new PagedSearchRequest
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

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });

            var model = new EditModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Discount = dto.Discount,
                Unit = dto.Unit,
                IsAvailable = dto.IsAvailable,
                Categories = await GetCategories(),
                ProductImages = dto.ProductImages.Select(
                    item =>
                        new ProductImageModel
                        {
                            Id = item.Id,
                            FileName = this._productImageAssetsManager.GetUrl(new GenericRequest<string> { Data = item.FileName }).Data,
                            IsPrimary = item.IsPrimary
                        }).ToList()
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
            var readResponse = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId=\"{tenantId}\" and Id={model.Id}"
            });

            if (!readResponse.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = readResponse.DtoCollection.First();

            dto.CategoryId = model.CategoryId;
            dto.Name = model.Name;
            dto.Description = model.Description;
            dto.IsAvailable = model.IsAvailable;
            dto.Price = model.Price;
            dto.Discount = model.Discount == null ? 0 : model.Discount.Value;
            dto.Unit = model.Unit;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._productService.UpdateAsync(new GenericRequest<ProductDto>
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
        public async Task<IActionResult> UploadProductImage(UploadProductImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var productResponse = await this._productService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantId,
                Data = model.ProductId
            });

            if (productResponse.IsError())
            {
                return this.GetErrorJson(productResponse);
            }

            if (productResponse.Data.ProductImages.Count >= CoreConstant.Settings.MaxProductImage)
            {
                return this.GetErrorJson(ProductResource.MaxProductImageReached);
            }

            var uploadResponse = await this.UploadImageBase64(model.Base64Logo, model.FileName);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            var productImageDto = new ProductImageDto
            {
                TenantId = tenantId,
                ProductId = model.ProductId,
                FileName = uploadResponse.ServerFileName,
                IsPrimary = !productResponse.Data.ProductImages.Any()
            };
            this.PopulateAuditFieldsOnCreate(productImageDto);

            var response = await this._productImageService.InsertAsync(new GenericRequest<ProductImageDto>
            {
                Data = productImageDto
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            var tenantShortName = this.User.Identity.GetTenantShortName();
            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });
            var fileNameResponse = this._productImageAssetsManager.GetUrl(new GenericRequest<string> { Data = response.Data.FileName });
            response.Data.FileName = fileNameResponse.Data;

            return this.GetSuccessJson(response, response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(ulong id)
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var response = await this._productImageService.TenantDeleteAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantId,
                Data = id
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            if (!response.Data.IsPrimary)
            {
                return this.GetSuccessJson(response, response.Data);
            }

            var productImageResponse = await this._productImageService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = CoreConstant.Settings.MaxProductImage,
                OrderByFieldName = "CreatedDateTime",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId={tenantId} and productId={response.Data.ProductId}"
            });
            var productImages = productImageResponse.DtoCollection;

            if (!productImages.Any())
            {
                return this.GetSuccessJson(response, response.Data);
            }

            var productImage = productImages.First();
            productImage.IsPrimary = true;
            this.PopulateAuditFieldsOnUpdate(productImage);
            var updateResponse = await this._productImageService.UpdateAsync(new GenericRequest<ProductImageDto> { 
                Data = productImage
            });

            if (updateResponse.IsError())
            {
                return this.GetErrorJson(updateResponse);
            }

            return this.GetSuccessJson(updateResponse, updateResponse.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SetPrimary(ulong id)
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var response = await this._productImageService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantId,
                Data = id
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            var productImageDto = response.Data;
            productImageDto.IsPrimary = true;
            this.PopulateAuditFieldsOnUpdate(productImageDto);

            var editResponse = await this._productImageService.SetPrimaryAsync(new GenericRequest<ProductImageDto>
            {
                Data = productImageDto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        #endregion

        #region Populate Grid Data

        private object PopulateProductResponse(ProductDto dto, string tenantShortName, IAssetsManagerBase assetsManager)
        {
            var fileName = dto.ProductImages.FirstOrDefault(item => item.IsPrimary)?.FileName;
            var imageUrlResponse = assetsManager.GetUrl(new GenericRequest<string> { Data = fileName });

            return new
            {
                dto.Id,
                dto.Name,
                dto.IsAvailable,
                dto.Price,
                dto.Discount,
                ImageUrl = imageUrlResponse.Data
            };
        }

        private async Task<SelectList> GetCategories()
        {
            var tenantId = this.User.Identity.GetTenantId();

            var response = await this._categoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId=\"{tenantId}\""
            });

            return new SelectList(response.DtoCollection, "Id", "Name");
        }

        private async Task<FileUploadResponse> UploadImageBase64(string base64PngImage, string fileName)
        {
            var tenantShortName = this.User.Identity.GetTenantShortName();
            var trimmedBase64Image = base64PngImage.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(trimmedBase64Image);
            using (var memoryStream = new MemoryStream(image))
            {
                this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });
                var uploadResponse = await this._productImageAssetsManager.UploadAsync(new FileUploadRequest
                {
                    FileStream = memoryStream,
                    FileName = fileName,
                    FileSize = (ulong)memoryStream.Length,
                    MimeType = "image/png"
                });

                return uploadResponse;
            }
        }

        #endregion
    }
}