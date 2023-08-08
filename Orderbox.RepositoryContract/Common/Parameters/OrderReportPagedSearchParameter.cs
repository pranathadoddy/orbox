using Framework.RepositoryContract;
using System;

namespace Orderbox.RepositoryContract.Common.Parameters
{
    public class OrderReportPagedSearchParameter : PagedSearchParameter
    {
        public int TenantId { get; set; }
        public int CategoryId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
