using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Orderbox.Mvc.Models.Voucher
{
    public class IndexModel
    {
        public string Name { get; set; }

        public string VoucherCode { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; }

        public string TermAndCondition { get; set; }

        public DateTime ValidStartDate { get; set; }

        public DateTime ValidEndDate { get; set; }

        public bool IsValid { 
            get {
                var today = DateTime.Now;
                return today >= ValidStartDate && today <= ValidEndDate;
            } 
        }

        public string RedeemMethod { get; set; }

        public DateTime RedeemDate { get; set; }

        public SelectList StoreLocationSelectList { get; set; }

        public List<StoreModel> StoreLocations { get; set; }

        public ulong StoreLocationId { get; set; }

        public string Code { get; set; }
    }
}