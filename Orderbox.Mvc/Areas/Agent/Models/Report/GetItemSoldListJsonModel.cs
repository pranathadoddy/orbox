using System;

namespace Orderbox.Mvc.Areas.Agent.Models.Report
{
    public class GetItemSoldListJsonModel
    {
        public DateTime date { get; set; }
        
        public int categoryId { get; set; }  
        
        public string keyword { get; set; }
        
        public int pageIndex { get; set; }
    }
}
