using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Report;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Report;
using Orderbox.ServiceContract.Report.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.Service.Report
{
    public class ReportService : IReportService
    {
        #region Fields

        private readonly IReportRepository _repository;

        #endregion

        #region Constructor

        public ReportService(IReportRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region Public Methods

        public async Task<GenericResponse<int>> GetTotalOrderSummaryAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericResponse<int>();

            response.Data = await _repository.GetTotalOrderSummaryAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericResponse<int>> GetTotalFinishedOrderSummaryAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericResponse<int>();

            response.Data = await _repository.GetTotalFinishedOrderSummaryAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericResponse<int>> GetTotalCancelledOrderSummaryAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericResponse<int>();

            response.Data = await _repository.GetTotalCancelledOrderSummaryAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericResponse<decimal>> GetTotalRevenueSummaryAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericResponse<decimal>();

            response.Data = await _repository.GetTotalRevenueSummaryAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericGetDtoCollectionResponse<ChartDataDto<int>>> GetPopularProductChartAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericGetDtoCollectionResponse<ChartDataDto<int>>();

            response.DtoCollection =
                    await _repository.GetPopularProductsAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericGetDtoCollectionResponse<ChartDataDto<int>>> GetProductTopRevenueChartAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericGetDtoCollectionResponse<ChartDataDto<int>>();

            response.DtoCollection =
                    await _repository.GetProductTopRevenueChartAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericGetDtoCollectionResponse<ChartDataDto<decimal>>> GetDailyRevenueChartAsync(GenericTenantRequest<DateTime> request)
        {
            var response = new GenericGetDtoCollectionResponse<ChartDataDto<decimal>>();

            response.DtoCollection =
                    await _repository.GetDailyRevenueAsync(request.TenantId, request.Data);

            return response;
        }

        public async Task<GenericResponse<List<OrderItemSoldDto>>> GetItemSoldSummaryAsync(ReportListPaginationRequest request) 
        {
            var response = new GenericResponse<List<OrderItemSoldDto>> ();

            response.Data = await _repository.GetItemSoldInMonthAsync(request.TenantId, request.Date, request.CategoryId);

            return response;
        }

        #endregion

    }
}
