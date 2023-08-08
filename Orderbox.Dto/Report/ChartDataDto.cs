namespace Orderbox.Dto.Report
{
    public class ChartDataDto<T>
    {
        public string Key { get; set; }

        public T Value { get; set; }

        public string Display { get; set; }
    }
}
