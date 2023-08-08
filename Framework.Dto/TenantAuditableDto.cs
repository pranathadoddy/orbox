namespace Framework.Dto
{
    public abstract class TenantAuditableDto<T> : AuditableDto<T>
    {
        public ulong TenantId { get; set; }
    }
}
