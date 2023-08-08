using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.ServiceContract.Voucher;
using Orderbox.Mvc.Models.Voucher;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Framework.Core.Resources;
using Orderbox.Core;
using Orderbox.Dto.Voucher;
using System;
using Orderbox.ServiceContract.Transaction;
using System.Text.Json;
using Orderbox.Dto.Location;
using System.Linq;

namespace Orderbox.Mvc.Controllers
{
    [Route("voucher")]
    public class VoucherController : BaseController
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IVoucherService _voucherService;

        public VoucherController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            IOrderItemService orderItemService,
            IVoucherService voucherService
        ) : base(configuration, hostEnvironment)
        {
            this._orderItemService = orderItemService;
            this._voucherService = voucherService;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Index(string code)
        {
            try
            {
                var id = Framework.Core.Cryptographer.Base64OTPDecrypt(code);

                var response = await this._voucherService.ReadAsync(new GenericRequest<ulong>
                {
                    Data = ulong.Parse(id)
                });

                if (response.IsError())
                {
                    return NotFound();
                }

                var voucherDto = response.Data;

                var orderItemResponse = await this._orderItemService.ReadAsync(new GenericRequest<ulong> { Data = voucherDto.OrderItemId });
                var orderItemDto = orderItemResponse.Data;
                var storeLocations = new List<StoreDto>();
                if (!string.IsNullOrEmpty(orderItemDto.AvailableRedeemStores))
                {
                    try
                    {
                        storeLocations = JsonSerializer.Deserialize<List<StoreDto>>(orderItemDto.AvailableRedeemStores);
                    }
                    catch { }
                }

                var model = new IndexModel
                {
                    Code = code,
                    Name = voucherDto.Name,
                    VoucherCode = voucherDto.VoucherCode,
                    Quantity = 1,
                    Status = voucherDto.Status,
                    ValidEndDate = voucherDto.ValidEndDate,
                    ValidStartDate = voucherDto.ValidStartDate,
                    TermAndCondition = voucherDto.TermAndCondition,
                    RedeemMethod = voucherDto.RedeemMethod,
                    RedeemDate = voucherDto.RedeemDate,
                    StoreLocationSelectList = this.PopulateStoreLocationAsync(storeLocations),
                    StoreLocations =
                        storeLocations
                            .Select(item => new StoreModel
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Address = item.Address,
                                MapUrl = item.MapUrl,
                                CityName = item.City.Name,
                                CountryName = item.City.Country.Name
                            })
                            .ToList(),
                    StoreLocationId = voucherDto.RedeemStoreId.HasValue ? voucherDto.RedeemStoreId.Value : 0
                };

                return View(model);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost("redeem")]
        public async Task<IActionResult> Redeem(RedeemModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            try
            {
                var id = Framework.Core.Cryptographer.Base64OTPDecrypt(model.Code);

                var response = await this._voucherService.ReadAsync(new GenericRequest<ulong>
                {
                    Data = ulong.Parse(id)
                });

                if (response.IsError())
                {
                    return this.GetErrorJson(response);
                }

                var voucherDto = response.Data;
                voucherDto.Status = CoreConstant.VoucherStatus.Used;
                voucherDto.RedeemDate = DateTime.UtcNow;
                voucherDto.RedeemStoreId = model.StoreLocationId;
                this.PopulateAuditFieldsOnUpdate(voucherDto);

                var editResponse = await this._voucherService.UpdateAsync(new GenericRequest<VoucherDto>
                {
                    Data = voucherDto
                });

                if (editResponse.IsError())
                {
                    return this.GetErrorJson(editResponse);
                }

                return this.GetBasicSuccessJson();
            }
            catch
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }
        }

        private SelectList PopulateStoreLocationAsync(List<StoreDto> storeDtos)
        {
            var options = new Dictionary<string, string>()
            {
                {"", "Pilih Lokasi Toko"}
            };

            foreach (var storeDto in storeDtos)
            {
                options.Add(storeDto.Id.ToString(), storeDto.Name);
            }

            return new SelectList(options, "Key", "Value");
        }
    }
}
