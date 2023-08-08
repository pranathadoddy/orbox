using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Mvc.Areas.User.Models.Report;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Report;
using Orderbox.ServiceContract.Report.Request;
using System;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class ReportController : BaseController
    {
        #region Fields

        private readonly IReportService _reportService;
        private readonly ITenantService _tenantService;

        #endregion

        #region Constructor

        public ReportController(
            IConfiguration configuration,
            IReportService reportService,
            ITenantService tenantService,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._reportService = reportService;
            this._tenantService = tenantService;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var response = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = tenantId });
            var tenantDto = response.Data;

            var model = new IndexModel()
            {
                DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                DateTo = DateTime.Now,
                TenantCurrency = tenantDto.Currency
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalOrderSummary(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpGet]
        public async Task<IActionResult> GetTotalFinishedOrderSummary(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpGet]
        public async Task<IActionResult> GetTotalCancelledOrderSummary(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpGet]
        public async Task<IActionResult> GetTotalRevenueSummary(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpGet]
        public async Task<IActionResult> GetPopularProductChart(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpGet]
        public async Task<IActionResult> GetProductTopRevenueChart(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpGet]
        public async Task<IActionResult> GetDailyRevenueChart(DateTime date)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());
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

        [HttpPost]
        public async Task<ActionResult> GetItemSoldListJson(DateTime date, ulong categoryId)
        {
            var tenantId = Convert.ToUInt64(this.User.Identity.GetTenantId());

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

        #endregion

    }
}