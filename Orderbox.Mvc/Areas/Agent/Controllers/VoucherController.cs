using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core;
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
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Voucher;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Voucher")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class VoucherController : BaseController
    {
        private readonly ITenantService _tenantService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IProductImageAssetsManager _productImageAssetsManager;
        private readonly IProductAgencyCategoryService _productAgencyCategoryService;
        private readonly IProductStoreService _productStoreService;

        public VoucherController(
            IConfiguration configuration,
            ITenantService tenantService,
            IProductService productService,
            IProductImageService productImageService,
            ICategoryService categoryService,
            IProductImageAssetsManager productImageAssetsManager,
            IProductAgencyCategoryService productAgencyCategoryService,
            IProductStoreService productStoreService,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._tenantService = tenantService;
            this._productService = productService;
            this._productImageService = productImageService;
            this._categoryService = categoryService;
            this._productImageAssetsManager = productImageAssetsManager;
            this._productAgencyCategoryService = productAgencyCategoryService;
            this._productStoreService = productStoreService;
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
                    ActivePage = "Voucher"
                },
                Currency = tenantDto.Currency
            };

            return View(model);
        }

        [HttpPost("PagedSearchGridJson")]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });

            var filters = model.Filters + $" and Type=\"{CoreConstant.ProductType.Voucher}\"";
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
                rowJsonData.Add(this.PopulateProductResponse(dto, this._productImageAssetsManager));
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
                    ActivePage = "Voucher"
                },
                IsAvailable = true,
                Unit = CoreConstant.Product.DefaultUnit,
                Discount = 0,
                Categories = await GetCategories(tenantId),
                RedeemMethods = GetRedeemMethods()
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


            var dto = new ProductDto
            {
                TenantId = model.TenantId,
                CategoryId = model.CategoryId,
                Type = CoreConstant.ProductType.Voucher,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Discount = model.Discount == null ? 0 : model.Discount.Value,
                Unit = model.Unit,
                IsAvailable = model.IsAvailable,
                Commission = model.Commision,
                TermAndCondition = model.TermAndCondition,
                ValidPeriodStart = DateTimeManager.LocalToUtcDateTime(CoreConstant.DefaultTimeZone, model.ValidPeriodStart),
                ValidPeriodEnd = DateTimeManager.LocalToUtcDateTime(CoreConstant.DefaultTimeZone, model.ValidPeriodEnd),
                RedeemMethod = model.RedeemMethod
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

            var productAgencyCategoryDtoCollection = new List<ProductAgencyCategoryDto>();
            var agencyId = ulong.Parse(this.User.Identity.GetAgencyId());

            foreach (var agencyCategoryId in model.AgencyCategoryIds)
            {
                var productAgencyCategoryDto = new ProductAgencyCategoryDto
                {
                    AgencyCategoryId = agencyCategoryId,
                    TenantId = model.TenantId,
                    ProductId = dto.Id,
                    AgencyId = agencyId,
                };

                this.PopulateAuditFieldsOnCreate(productAgencyCategoryDto);

                productAgencyCategoryDtoCollection.Add(productAgencyCategoryDto);
            }

            var productAgencyCategoryResponse = await this._productAgencyCategoryService.BulkInsert(new GenericRequest<ICollection<ProductAgencyCategoryDto>>
            {
                Data = productAgencyCategoryDtoCollection
            });

            if (productAgencyCategoryResponse.IsError())
            {
                return this.GetErrorJson(productAgencyCategoryResponse);
            }


            var productStoreDtoCollection = new List<ProductStoreDto>();

            foreach (var storeId in model.StoreIds)
            {
                var productStoreDto = new ProductStoreDto
                {
                    StoreId = storeId,
                    TenantId = model.TenantId,
                    ProductId = dto.Id,
                };

                this.PopulateAuditFieldsOnCreate(productStoreDto);

                productStoreDtoCollection.Add(productStoreDto);
            }

            var productStoreResponse = await this._productStoreService.BulkInsert(new GenericRequest<ICollection<ProductStoreDto>>
            {
                Data = productStoreDtoCollection
            });

            if (productStoreResponse.IsError())
            {
                return this.GetErrorJson(productStoreResponse);
            }

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Agent/Voucher/Index/{model.TenantId}"
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

            var response = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId=\"{tenantId}\" and id={id}"
            });

            if (!response.DtoCollection.Any())
            {
                return NotFound();
            }

            var dto = response.DtoCollection.First();

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });

            var model = new EditModel
            {
                TenantId = tenantId,
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Voucher"
                },
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Discount = dto.Discount,
                Unit = dto.Unit,
                IsAvailable = dto.IsAvailable,
                Categories = await GetCategories(tenantId),
                ProductImages = dto.ProductImages.Select(
                    item =>
                        new ProductImageModel
                        {
                            Id = item.Id,
                            FileName = this._productImageAssetsManager.GetUrl(new GenericRequest<string> { Data = item.FileName }).Data,
                            IsPrimary = item.IsPrimary
                        }).ToList(),
                Commision = dto.Commission,
                RedeemMethod = dto.RedeemMethod,
                TermAndCondition = dto.TermAndCondition,
                ValidPeriodStart = DateTimeManager.UtcToLocalDateTime(CoreConstant.DefaultTimeZone, dto.ValidPeriodStart),
                ValidPeriodEnd = DateTimeManager.UtcToLocalDateTime(CoreConstant.DefaultTimeZone, dto.ValidPeriodEnd),
                RedeemMethods = GetRedeemMethods(),
                ProductAgencyCategories = await GetProductAgencyCategories(dto.Id),
                ProductStores = await GetProductStore(dto.Id),
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

            var readResponse = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId=\"{model.TenantId}\" and id={model.Id}"
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
            dto.Commission = model.Commision;
            dto.RedeemMethod = model.RedeemMethod;
            dto.TermAndCondition = model.TermAndCondition;
            dto.ValidPeriodStart = DateTimeManager.LocalToUtcDateTime("Singapore Standard Time", model.ValidPeriodStart);
            dto.ValidPeriodEnd = DateTimeManager.LocalToUtcDateTime("Singapore Standard Time", model.ValidPeriodEnd);

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._productService.UpdateAsync(new GenericRequest<ProductDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            await this._productAgencyCategoryService.DeleteByProductIdAsync(new GenericRequest<ulong> { Data = dto.Id });

            var productAgencyCategoryDtoCollection = new List<ProductAgencyCategoryDto>();
            var agencyId = ulong.Parse(this.User.Identity.GetAgencyId());

            foreach (var agencyCategoryId in model.AgencyCategoryIds)
            {
                var productAgencyCategoryDto = new ProductAgencyCategoryDto
                {
                    AgencyCategoryId = agencyCategoryId,
                    TenantId = model.TenantId,
                    ProductId = dto.Id,
                    AgencyId = agencyId,
                };

                this.PopulateAuditFieldsOnCreate(productAgencyCategoryDto);

                productAgencyCategoryDtoCollection.Add(productAgencyCategoryDto);
            }

            var productAgencyCategoryResponse = await this._productAgencyCategoryService.BulkInsert(new GenericRequest<ICollection<ProductAgencyCategoryDto>>
            {
                Data = productAgencyCategoryDtoCollection
            });

            if (productAgencyCategoryResponse.IsError())
            {
                return this.GetErrorJson(productAgencyCategoryResponse);
            }

            await this._productStoreService.DeleteByProductIdAsync(new GenericRequest<ulong> { Data = dto.Id });

            var productStoreDtoCollection = new List<ProductStoreDto>();

            foreach (var storeId in model.StoreIds)
            {
                var productStoreDto = new ProductStoreDto
                {
                    StoreId = storeId,
                    TenantId = model.TenantId,
                    ProductId = dto.Id,
                };

                this.PopulateAuditFieldsOnCreate(productStoreDto);

                productStoreDtoCollection.Add(productStoreDto);
            }

            var productStoreResponse = await this._productStoreService.BulkInsert(new GenericRequest<ICollection<ProductStoreDto>>
            {
                Data = productStoreDtoCollection
            });

            if (productStoreResponse.IsError())
            {
                return this.GetErrorJson(productStoreResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        [HttpPost("UploadProductImage")]
        public async Task<IActionResult> UploadProductImage(UploadProductImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var tenantDto = tenant as TenantDto;

            var productResponse = await this._productService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = model.TenantId,
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

            var uploadResponse = await this.UploadImageBase64(model.Base64Logo, model.FileName, model.TenantId);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            var productImageDto = new ProductImageDto
            {
                TenantId = model.TenantId,
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

            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            var fileNameResponse = this._productImageAssetsManager.GetUrl(new GenericRequest<string> { Data = response.Data.FileName });
            response.Data.FileName = fileNameResponse.Data;

            return this.GetSuccessJson(response, response.Data);
        }

        [HttpPost("DeleteImage/{tenantId}/{id}")]
        public async Task<IActionResult> DeleteImage(ulong tenantId, ulong id)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

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
            var updateResponse = await this._productImageService.UpdateAsync(new GenericRequest<ProductImageDto>
            {
                Data = productImage
            });

            if (updateResponse.IsError())
            {
                return this.GetErrorJson(updateResponse);
            }

            return this.GetSuccessJson(updateResponse, updateResponse.Data);
        }

        [HttpPost("SetPrimary/{tenantId}/{id}")]
        public async Task<IActionResult> SetPrimary(ulong tenantId, ulong id)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

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

        private object PopulateProductResponse(ProductDto dto, IAssetsManagerBase assetsManager)
        {
            var fileName = dto.ProductImages.FirstOrDefault(item => item.IsPrimary)?.FileName;
            var imageUrlResponse = assetsManager.GetUrl(new GenericRequest<string> { Data = fileName });

            return new
            {
                dto.Id,
                dto.Type,
                dto.Name,
                dto.IsAvailable,
                dto.Price,
                dto.Discount,
                ImageUrl = imageUrlResponse.Data
            };
        }

        private async Task<FileUploadResponse> UploadImageBase64(string base64PngImage, string fileName, ulong id)
        {
            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = id });
            var tenant = tenantResponse.Data;
            var trimmedBase64Image = base64PngImage.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(trimmedBase64Image);
            using (var memoryStream = new MemoryStream(image))
            {
                this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenant.ShortName });
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

        private async Task<SelectList> GetCategories(ulong id)
        {
            var response = await this._categoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Name",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId=\"{id}\""
            });

            return new SelectList(response.DtoCollection, "Id", "Name");
        }

        private SelectList GetRedeemMethods()
        {
            var redeemMethodDictionary = RedeemMethodCode.Item.GetCodeList();
            return new SelectList(redeemMethodDictionary, "Value", "Description");
        }

        private async Task<SelectList> GetProductAgencyCategories(ulong productId)
        {
            var response = await this._productAgencyCategoryService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"ProductId=\"{productId}\""
            });

            return new SelectList(response.DtoCollection, "AgencyCategoryId", "AgencyCategoryName");
        }

        private async Task<SelectList> GetProductStore(ulong productId)
        {
            var response = await this._productStoreService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"ProductId=\"{productId}\""
            });

            return new SelectList(response.DtoCollection.Select(item => new { StoreId = item.StoreId, StoreName = item.Store.Name }), "StoreId", "StoreName");
        }

    }

}