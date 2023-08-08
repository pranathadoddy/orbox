namespace Orderbox.Mvc.Areas.Tenant.Models.Order
{
    public class UploadFileModel
    {
        #region Properties

        public ulong OrderId { get; set; }

        public string Base64File { get; set; }

        public string FileName { get; set; }

        #endregion
    }
}
