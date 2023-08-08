using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComAgent
    {
        public ulong Id { get; set; }
        public string Email { get; set; }
        public ulong AgencyId { get; set; }
        public string UserId { get; set; }
        public string Privilege { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}
