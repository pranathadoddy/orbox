namespace Orderbox.Mvc.Areas.Agent.Models.Profile
{
    public class UploadFileModel
    {
        #region Properties

        public ulong TenantId { get; set; }

        public string Base64File { get; set; }

        public string FileName { get; set; }

        #endregion
    }
}
