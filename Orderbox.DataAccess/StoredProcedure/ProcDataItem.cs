namespace Orderbox.DataAccess.StoredProcedure
{
    public partial class ProcDataItem<T1, T2>
    {
        public ProcDataItem()
        {

        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string TenantShortName { get; set; }
        public string Currency { get; set; }
        public string PrimaryImageFileName { get; set; }
        public int TotalData { get; set; }
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
    }
}
