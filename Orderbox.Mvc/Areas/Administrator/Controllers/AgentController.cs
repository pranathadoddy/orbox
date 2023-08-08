using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Administrator.Models.Agent;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Email;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class AgentController : BaseController
    {
        private readonly IAgencyService _agencyService;
        private readonly IAgentService _agentService;
        private readonly IAgentInvitationEmailManager _agentInvitationEmailManager;

        public AgentController(
            IConfiguration configuration,
            IAgencyService agencyService,
            IAgentService dealsAgentService,
            IWebHostEnvironment webHostEnvironment,
            IAgentInvitationEmailManager agentInvitationEmailManager) : 
            base(configuration, webHostEnvironment)
        {
            this._agencyService = agencyService;
            this._agentService = dealsAgentService;
            this._agentInvitationEmailManager = agentInvitationEmailManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model, ulong id)
        {

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"AgencyId = {id}";
            var response = await this._agentService.PagedSearchAsync(new PagedSearchRequest
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
                rowJsonData.Add(new
                {
                    dto.Id,
                    dto.Email,
                    Status = AgentStatusCode.Item.GetDescription(dto.Status),
                    Privilege = AgentPrivilegeCode.Item.GetDescription(dto.Privilege)
                });
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public IActionResult Create(ulong id)
        {
            var model = new CreateModel
            {
                Privileges = GetPrivileges(),
                AgencyId = id
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

            var agencyResponse = await this._agencyService.ReadAsync(new GenericRequest<ulong>
            {
                Data = model.AgencyId
            });

            if (agencyResponse.IsError())
            {
                return this.GetErrorJson(agencyResponse);
            }

            var dto = new AgentDto
            {
                Email = model.Email,
                AgencyId = model.AgencyId,
                Privilege = model.Privilege,
                Status = CoreConstant.AgentStatus.Activating
            };

            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._agentService.InsertAsync(new GenericRequest<AgentDto>
            {
                Data = dto
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            var agencyDto = agencyResponse.Data;
            var agentDto = response.Data;

            await SendActivationEmailAsync(agencyDto, agentDto);

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Administrator/Agency/Edit/{model.AgencyId}"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var response = await this._agentService.PagedSearchAsync(new PagedSearchRequest
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

            if (dto.Status == CoreConstant.AgentStatus.Activating)
            {
                var editModel = new EditModel
                {
                    Id = dto.Id,
                    Email = dto.Email,
                    AgencyId = dto.AgencyId,
                    Privilege = dto.Privilege,
                    Privileges = GetPrivileges()
                };

                return View("Edit", editModel);
            }

            var viewModel = new ViewModel
            {
                Id = dto.Id,
                Email = dto.Email,
                AgencyId = dto.AgencyId,
                Privilege = dto.Privilege,
                Privileges = GetPrivileges()
            };

            return View("View", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model)
        {
            var readResponse = await this._agentService.PagedSearchAsync(new PagedSearchRequest
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

            dto.Email = model.Email;
            dto.Privilege = model.Privilege;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._agentService.UpdateAsync(new GenericRequest<AgentDto>
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

            var readResponse = await this._agentService.PagedSearchAsync(new PagedSearchRequest
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

            await _agentService.DeleteAsync(new GenericRequest<ulong>
            {
                Data = id
            });

            return Json(new
            {
                IsSuccess = true,
                RedirectUrl = $"/Administrator/Agency/Edit/{dto.AgencyId}"
            });
        }

        private SelectList GetPrivileges()
        {
            var privileges = AgentPrivilegeCode.Item.GetCodeList();

            return new SelectList(privileges, "Value", "Description");
        }

        private async Task SendActivationEmailAsync(AgencyDto agencyDto, AgentDto agentDto)
        {
            var expirationSeconds = DateTimeManager.GetSecondSinceEpoch(1);
            var agentInvitationJwt = Cryptographer.JsonWebTokenEncode(new Dictionary<string, object> {
                {"exp", expirationSeconds},
                {"EmailAddress", agentDto.Email},
                {"AgentId", agentDto.Id}
            });
            var urlFormat = this.Configuration.GetValue<string>("Application:UrlFormat");
            var rootDomain = this.Configuration.GetValue<string>("Application:RootDomain");
            var path = $"/Account/AgentActivation?token={agentInvitationJwt}";
            var url = $"{string.Format(urlFormat, rootDomain)}{path}";

            var replacementTemplateData = JsonSerializer.Serialize(new Dictionary<string, string>
            {
                { "AGENCY_NAME", agencyDto.Name },
                { "ACCOUNT_ACTIVATION_URL", url }
            });

            this._agentInvitationEmailManager.Recipients.Add(new Dictionary<string, string>
            {
                { "ToAddress", agentDto.Email },
                { "ReplacementTemplateData",  replacementTemplateData }
            });

            await this._agentInvitationEmailManager.SendAsync();
        }
    }
}