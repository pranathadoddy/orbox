using System;

namespace Orderbox.ServiceContract.Report.Request
{
    public class ReportListPaginationRequest
    {
        public ulong TenantId { get; set; }

        public ulong CategoryId { get; set; }
        
        public DateTime Date { get; set; }
    }
}
