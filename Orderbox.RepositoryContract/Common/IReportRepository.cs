using Orderbox.Dto.Report;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.RepositoryContract.Common
{
    public interface IReportRepository
    {
        Task<int> GetTotalOrderSummaryAsync(ulong tenantId, DateTime date);

        Task<int> GetTotalFinishedOrderSummaryAsync(ulong tenantId, DateTime date);

        Task<int> GetTotalCancelledOrderSummaryAsync(ulong tenantId, DateTime date);

        Task<decimal> GetTotalRevenueSummaryAsync(ulong tenantId, DateTime date);

        Task<ICollection<ChartDataDto<int>>> GetPopularProductsAsync(ulong tenantId, DateTime date);

        Task<ICollection<ChartDataDto<int>>> GetProductTopRevenueChartAsync(ulong tenantId, DateTime date);

        Task<ICollection<ChartDataDto<decimal>>> GetDailyRevenueAsync(ulong tenantId, DateTime date);

        Task<List<OrderItemSoldDto>> GetItemSoldInMonthAsync(ulong tenantId, DateTime date, ulong categoryId);
    }
}
