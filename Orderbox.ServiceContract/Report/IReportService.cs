using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Report;
using Orderbox.ServiceContract.Report.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Report
{
    public interface IReportService
    {
        Task<GenericResponse<int>> GetTotalOrderSummaryAsync(GenericTenantRequest<DateTime> request);

        Task<GenericResponse<int>> GetTotalFinishedOrderSummaryAsync(GenericTenantRequest<DateTime> request);

        Task<GenericResponse<int>> GetTotalCancelledOrderSummaryAsync(GenericTenantRequest<DateTime> request);

        Task<GenericResponse<decimal>> GetTotalRevenueSummaryAsync(GenericTenantRequest<DateTime> request);

        Task<GenericGetDtoCollectionResponse<ChartDataDto<int>>> GetPopularProductChartAsync(GenericTenantRequest<DateTime> request);

        Task<GenericGetDtoCollectionResponse<ChartDataDto<int>>> GetProductTopRevenueChartAsync(GenericTenantRequest<DateTime> request);

        Task<GenericGetDtoCollectionResponse<ChartDataDto<decimal>>> GetDailyRevenueChartAsync(GenericTenantRequest<DateTime> request);

        Task<GenericResponse<List<OrderItemSoldDto>>> GetItemSoldSummaryAsync(ReportListPaginationRequest request);
    }
}
