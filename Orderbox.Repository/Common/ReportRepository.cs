using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.DataAccess.Application;
using Orderbox.Dto.Report;
using Orderbox.RepositoryContract.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Orderbox.Repository.Common
{
    public class ReportRepository : IReportRepository
    {
        #region Constant

        private const string GetTotalRevenueSummaryCommand = "CALL rpt_usp_GetTotalRevenueInMonth(@p0, @p1, @p2)";
        private const string GetPopularProductCommand = "CALL rpt_usp_GetSummaryPopularProduct(@p0, @p1, @p2, @p3)";
        private const string GetProductTopRevenueCommand = "CALL rpt_usp_GetProductTopRevenue(@p0, @p1, @p2, @p3)";
        private const string GetDailyRevenueCommand = "CALL rpt_usp_GetDailyRevenueChart(@p0, @p1)";
        private const string GetItemSoldDetailListCommand = "CALL rpt_usp_GetItemSoldDetailListInMonth(@p0, @p1, @p2, @p3)";

        private OrderboxContext Context { get; }

        private IConfiguration _configuration;

        #endregion

        #region constructor

        public ReportRepository(OrderboxContext context, IConfiguration configuration)
        {
            Context = context;
            this._configuration = configuration;
        }

        #endregion

        #region Public Methods

        public async Task<int> GetTotalOrderSummaryAsync(ulong tenantId, DateTime date)
        {
            var dbSet = this.Context.Set<TrxOrder>();
            var total =
                await dbSet.Where(
                    item =>
                        item.TenantId == tenantId &&
                        item.Date.Month == date.Month &&
                        item.Date.Year == date.Year).CountAsync();

            return total;
        }

        public async Task<int> GetTotalFinishedOrderSummaryAsync(ulong tenantId, DateTime date)
        {
            var dbSet = this.Context.Set<TrxOrder>();
            var total =
                await dbSet.Where(
                    item =>
                        item.TenantId == tenantId &&
                        item.Status == CoreConstant.OrderStatus.Finished &&
                        item.Date.Month == date.Month &&
                        item.Date.Year == date.Year).CountAsync();

            return total;
        }

        public async Task<int> GetTotalCancelledOrderSummaryAsync(ulong tenantId, DateTime date)
        {
            var dbSet = this.Context.Set<TrxOrder>();
            var total =
                await dbSet.Where(
                    item =>
                        item.TenantId == tenantId &&
                        item.Status == CoreConstant.OrderStatus.Cancelled &&
                        item.Date.Month == date.Month &&
                        item.Date.Year == date.Year).CountAsync();

            return total;
        }

        public async Task<decimal> GetTotalRevenueSummaryAsync(ulong tenantId, DateTime date)
        {
            var entities =
                    await
                        this.Context
                            .ProcRevenueSummary
                            .FromSqlRaw(GetTotalRevenueSummaryCommand, tenantId, date.Month, date.Year)
                            .ToListAsync();

            var entity = entities.AsQueryable().First();

            return entity.Value;
        }

        public async Task<ICollection<ChartDataDto<int>>> GetPopularProductsAsync(ulong tenantId, DateTime date)
        {
            return
                await
                    this.Context
                        .ProcPopularProductChart
                        .FromSqlRaw(GetPopularProductCommand, tenantId, date.Month, date.Year, CoreConstant.DoughnutChart.MaxMainItemNumber)
                        .Select(pp => new ChartDataDto<int>
                        {
                            Key = pp.Key,
                            Value = pp.Value,
                            Display = pp.Display
                        })
                        .ToListAsync();
        }

        public async Task<ICollection<ChartDataDto<int>>> GetProductTopRevenueChartAsync(ulong tenantId, DateTime date)
        {
            return
                await
                    this.Context
                        .ProcPopularProductChart
                        .FromSqlRaw(GetProductTopRevenueCommand, tenantId, date.Month, date.Year, CoreConstant.DoughnutChart.MaxMainItemNumber)
                        .Select(pp => new ChartDataDto<int>
                        {
                            Key = pp.Key,
                            Value = pp.Value,
                            Display = pp.Display
                        })
                        .ToListAsync();
        }

        public async Task<ICollection<ChartDataDto<decimal>>> GetDailyRevenueAsync(ulong tenantId, DateTime date)
        {
            return
                await
                    this.Context
                        .ProcDailyRevenueChart
                        .FromSqlRaw(GetDailyRevenueCommand, tenantId, date)
                        .Select(pp => new ChartDataDto<decimal>
                        {
                            Key = pp.Key,
                            Value = pp.Value,
                            Display = pp.Display
                        })
                        .ToListAsync();
        }

        public async Task<List<OrderItemSoldDto>> GetItemSoldInMonthAsync(ulong tenantId, DateTime date, ulong categoryId)
        {
            var cdnUrl = this._configuration.GetValue<string>("DOSpace:Cdn");
            var directory = this._configuration.GetValue<string>("DOSpace:MainDirectory");

            return
                await
                    this.Context
                        .ProcItemSoldList
                        .FromSqlRaw(GetItemSoldDetailListCommand, tenantId, date.Month, date.Year, categoryId)
                        .Select(i => new OrderItemSoldDto()
                        {
                            Id = i.Id,
                            ProductName = i.Name,
                            UnitPrice = i.Value1,
                            Quantity = i.Value2,
                            Unit = i.Unit,
                            Currency = i.Currency,
                            TenantShortName = i.TenantShortName,
                            PrimaryImageFileName = $"{cdnUrl}/{directory}/{i.TenantShortName}/{i.PrimaryImageFileName}",
                            TotalData = i.TotalData
                        })
                        .ToListAsync();
        }

        #endregion
    }
}
