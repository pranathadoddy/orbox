using Microsoft.EntityFrameworkCore;
using Orderbox.DataAccess.StoredProcedure;

namespace Orderbox.DataAccess.Application
{
    public partial class OrderboxContext : DbContext
    {
        public virtual DbSet<ProcDataSummary<int>> ProcTotalTransactionSummary { get; set; }

        public virtual DbSet<ProcDataSummary<int>> ProcItemSalesSummary { get; set; }

        public virtual DbSet<ProcDataSummary<decimal>> ProcRevenueSummary { get; set; }

        public virtual DbSet<ProcDataChart<int>> ProcPopularProductChart { get; set; }

        public virtual DbSet<ProcDataChart<decimal>> ProcTopProductRevenueChart { get; set; }

        public virtual DbSet<ProcDataChart<decimal>> ProcDailyRevenueChart { get; set; }

        public virtual DbSet<ProcDataItem<decimal, int>> ProcItemSoldList { get; set; }
    }
}
