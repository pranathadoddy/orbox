using Framework.ServiceContract.Response;

namespace Framework.ServiceContract.FileUpload.Response
{
    public class FileUploadResponse : BasicResponse
    {
        public string Name { get; set; }

        public string ServerFileName { get; set; }

        public string Url { get; set; }

        public string MimeType { get; set; }

        public ulong Size { get; set; }
    }
}
