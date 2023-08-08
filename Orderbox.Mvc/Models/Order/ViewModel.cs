using Framework.Application.Presentation;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Order
{
    public class ViewModel
    {
        #region Properties

        public ulong Id { get; set; }

        [Required]
        public string Status { get; set; }
        
        public IEnumerable<RadioItem> StatusRadioItems { get; set; }

        public OrderDto Order { get; set; }

        public string Currency { get; set; }

        #endregion
    }
}
