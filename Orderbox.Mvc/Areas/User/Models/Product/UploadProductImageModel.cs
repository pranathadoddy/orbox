namespace Orderbox.Mvc.Areas.User.Models.Product
{
    public class UploadProductImageModel
    {
        public string Base64Logo { get; set; }

        public string FileName { get; set; }

        public ulong ProductId { get; set; }
    }
}
