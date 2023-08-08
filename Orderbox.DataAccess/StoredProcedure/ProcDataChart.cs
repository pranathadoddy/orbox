namespace Orderbox.DataAccess.StoredProcedure
{
    public partial class ProcDataChart<T>
    {
        public ProcDataChart()
        {
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public T Value { get; set; }
        public string Display { get; set; }
    }
}
