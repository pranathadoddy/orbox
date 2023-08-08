namespace Orderbox.DataAccess.StoredProcedure
{
    public partial class ProcDataSummary<T>
    {
        public ProcDataSummary()
        {
        }

        public int Id { get; set; }

        public T Value { get; set; }
    }
}
