namespace Framework.ServiceContract.Request
{
    public class GenericTenantRequest<T> : GenericRequest<T>
    {
        public ulong TenantId { get; set; }
    }
}
