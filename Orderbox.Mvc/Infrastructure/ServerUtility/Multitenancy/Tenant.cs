using System.Collections.Generic;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy
{
    public class Tenant
    {
        public ulong Id { get; set; }

        public string Domain { get; set; }

        public Dictionary<string, object> Items { get; private set; } = new Dictionary<string, object>();
    }
}
