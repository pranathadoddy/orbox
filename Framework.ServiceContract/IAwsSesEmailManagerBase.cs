using Framework.ServiceContract.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.ServiceContract
{
    public interface IAwsSesEmailManagerBase
    {
        ICollection<Dictionary<string, string>> Recipients { get; }

        string TemplateFilePath { get; set; }        

        Task BulkSendAsync();

        void Clear();

        Task DeleteTemplateAsync();

        Task SendAsync();

        Task<BasicResponse> UploadTemplateAsync();
    }
}
