using System;
using System.Collections.Generic;
using System.Text;

namespace Orderbox.ServiceContract.Common.Request
{
    public class IsDomainExistRequest
    {
        public string DomainPostfix { get; set; }

        public ulong Id { get; set; }
    }
}
