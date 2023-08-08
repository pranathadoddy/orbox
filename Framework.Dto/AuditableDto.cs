using System;

namespace Framework.Dto
{
    public abstract class AuditableDto<T> : BaseDto<T>
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDateTime { get; set; }
    }
}
