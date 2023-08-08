namespace Orderbox.Dto.Report
{
    public class OrderItemSoldDto
    {
        public ulong Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalSales { get { return (Quantity * UnitPrice); } }
        public string TenantShortName { get; set; }
        public string Currency { get; set; }
        public string PrimaryImageFileName { get; set; }
        public int TotalData { get; set; }
    }
}
