﻿namespace Orderbox.Mvc.Areas.Agent.Models.Product
{
    public class UploadProductImageModel
    {
        public ulong TenantId { get; set; }

        public string Base64Logo { get; set; }

        public string FileName { get; set; }

        public ulong ProductId { get; set; }
    }
}
