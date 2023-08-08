namespace Framework.ServiceContract.Request
{
    public class GenericWithCustomerRequest<T> : GenericRequest<T>
    {
        public ulong CustomerId { get; set; }
    }
}
