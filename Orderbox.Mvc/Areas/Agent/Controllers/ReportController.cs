using Framework.Application.Controllers;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Report;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.ServiceContract.Report;
using Orderbox.ServiceContract.Report.Request;
using System;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Report")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(
            IConfiguration configuration,
            IReportService reportService,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._reportService = reportService;
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
                    ActivePage = "Report"
                },
                TenantCurrency = tenantDto.Currency
            };

            return View(model);
        }

        [HttpGet("GetTotalOrderSummary/{tenantId}/{date}")]
        public async Task<IActionResult> GetTotalOrderSummary(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetTotalOrderSummaryAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, new
            {
                Type = CoreConstant.SummaryType.SummaryInteger,
                Result = response.Data
            });
        }

        [HttpGet("GetTotalFinishedOrderSummary/{tenantId}/{date}")]
        public async Task<IActionResult> GetTotalFinishedOrderSummary(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetTotalFinishedOrderSummaryAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, new
            {
                Type = CoreConstant.SummaryType.SummaryInteger,
                Result = response.Data
            });
        }

        [HttpGet("GetTotalCancelledOrderSummary/{tenantId}/{date}")]
        public async Task<IActionResult> GetTotalCancelledOrderSummary(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetTotalCancelledOrderSummaryAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, new
            {
                Type = CoreConstant.SummaryType.SummaryInteger,
                Result = response.Data
            });
        }

        [HttpGet("GetTotalRevenueSummary/{tenantId}/{date}")]
        public async Task<IActionResult> GetTotalRevenueSummary(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetTotalRevenueSummaryAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, new
            {
                Type = CoreConstant.SummaryType.SummaryMoney,
                Result = response.Data
            });
        }

        [HttpGet("GetPopularProductChart/{tenantId}/{date}")]
        public async Task<IActionResult> GetPopularProductChart(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetPopularProductChartAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.DtoCollection);
        }

        [HttpGet("GetProductTopRevenueChart/{tenantId}/{date}")]
        public async Task<IActionResult> GetProductTopRevenueChart(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetProductTopRevenueChartAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.DtoCollection);
        }

        [HttpGet("GetDailyRevenueChart/{tenantId}/{date}")]
        public async Task<IActionResult> GetDailyRevenueChart(ulong tenantId, DateTime date)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._reportService.GetDailyRevenueChartAsync(new GenericTenantRequest<DateTime>
            {
                TenantId = tenantId,
                Data = date
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.DtoCollection);
        }

        [HttpGet("GetItemSoldListJson/{tenantId}/{categoryId}/{date}")]
        public async Task<ActionResult> GetItemSoldListJson(ulong tenantId, ulong categoryId, DateTime date)
        {
            var response = await this._reportService.GetItemSoldSummaryAsync(new ReportListPaginationRequest
            {
                TenantId = tenantId,
                Date = date,
                CategoryId = categoryId
            });

            if (response.IsError())
            {
                this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, new
            {
                Type = CoreConstant.SummaryType.SummaryItemList,
                Result = response.Data
            });
        }
    }
}