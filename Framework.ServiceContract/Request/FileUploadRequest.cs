using System.IO;

namespace Framework.ServiceContract.FileUpload.Request
{
    public class FileUploadRequest
    {
        public Stream FileStream { get; set; }

        public string FileName { get; set; }

        public ulong FileSize { get; set; }

        public string MimeType { get; set; }

    }
}
