using Framework.Application.Controllers;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.User.Models.Tenant;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class TenantController : BaseController
    {
        private readonly ITenantPostNotificationTokenService _tenantPostNotificationTokenService;

        public TenantController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ITenantPostNotificationTokenService tenantPostNotificationTokenService) :
            base(configuration, hostEnvironment)
        {
            this._tenantPostNotificationTokenService = tenantPostNotificationTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdatePostNotificationToken(AddOrUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var response = await this._tenantPostNotificationTokenService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = string.Empty,
                Filters = $"tenantId={tenantId}"
            });

            if (!response.DtoCollection.ToList().Any())
            {
                return await this.InsertTokenAsync(model, tenantId);
            }

            var dto = response.DtoCollection.FirstOrDefault();
            return await this.UpdateTokenAsync(model, dto);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePostNotificationToken()
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var response = await this._tenantPostNotificationTokenService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = CoreConstant.SortOrder.Ascending,
                Keyword = string.Empty,
                Filters = $"tenantId={tenantId}"
            });

            if (!response.DtoCollection.ToList().Any())
            {
                return this.GetErrorJson(GeneralResource.Item_Delete_NotFound);
            }

            var dto = response.DtoCollection.FirstOrDefault();
            var deleteResponse = await this._tenantPostNotificationTokenService.TenantDeleteAsync(new GenericTenantRequest<ulong> { 
                TenantId = tenantId,
                Data = dto.Id
            });

            if (deleteResponse.IsError())
            {
                return this.GetErrorJson(deleteResponse);
            }

            return this.GetSuccessJson(deleteResponse, deleteResponse.Data);
        }

        private async Task<IActionResult> UpdateTokenAsync(AddOrUpdateModel model, TenantPushNotificationTokenDto dto)
        {
            dto.Token = model.Token;
            this.PopulateAuditFieldsOnUpdate(dto);

            var response = await this._tenantPostNotificationTokenService.UpdateAsync(new GenericRequest<TenantPushNotificationTokenDto> { Data = dto });
            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.Data);
        }

        private async Task<IActionResult> InsertTokenAsync(AddOrUpdateModel model, ulong tenantId)
        {
            var dto = new TenantPushNotificationTokenDto
            {
                TenantId = tenantId,
                Token = model.Token
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._tenantPostNotificationTokenService.InsertAsync(
                new GenericRequest<TenantPushNotificationTokenDto>
                {
                    Data = dto
                });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.Data);
        }
    }
}
