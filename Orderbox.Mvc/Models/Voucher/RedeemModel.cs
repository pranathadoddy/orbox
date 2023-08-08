using Framework.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Voucher
{
    public class RedeemModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [Range(1, ulong.MaxValue, ErrorMessageResourceType = typeof(GeneralResource), ErrorMessageResourceName = "General_EnterValueBiggerThan")]
        public ulong StoreLocationId { get; set; }
    }
}
