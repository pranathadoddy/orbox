using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Api.Infrastructure.ServerUtility.Identity;
using Orderbox.Api.Models.Customer;
using Orderbox.Core.Resources.Common;
using Orderbox.Dto.Common;
using Orderbox.ServiceContract.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Api.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Google")]
    public class CustomerController : ApiBaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ICustomerService customerService
        ) : base(configuration, hostEnvironment)
        {
            this._customerService = customerService;
        }

        [HttpGet("external/{provider}")]
        public async Task<IActionResult> GetByProvider(string provider)
        {
            var externalId = this.User.Identity.GetUserId();
            var authType = System.Net.WebUtility.UrlDecode(provider);

            var response = await this._customerService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"authType=\"{authType}\" and externalId=\"{externalId}\""
            });

            if (response.TotalCount == 0)
            {
                return this.NotFound();
            }

            return this.GetSuccessJson(new BasicResponse(), response.DtoCollection.First());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var externalId = this.User.Identity.GetUserId();
            var authType = this.User.Identity.GetIssuer();

            var response = await this._customerService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"authType=\"{authType}\" and externalId=\"{externalId}\""
            });

            if (response.TotalCount > 0)
            {
                return this.GetErrorJson(CustomerResource.CustomerExist);
            }

            var profilePicture = this.User.Identity.GetPicture();

            var dto = new CustomerDto
            {
                AuthType = authType,
                ExternalId = externalId,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                ProfilePicture = profilePicture
            };
            this.PopulateAuditFieldsOnCreate(dto);

            var createResponse = await this._customerService.InsertAsync(new GenericRequest<CustomerDto> { Data = dto });

            if (createResponse.IsError())
            {
                return this.GetErrorJson(createResponse);
            }

            return this.GetSuccessJson(createResponse, createResponse.Data);
        }
    }
}
