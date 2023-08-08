using System.Threading.Tasks;
using Framework.ServiceContract.FileUpload.Request;
using Framework.ServiceContract.FileUpload.Response;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;

namespace Framework.ServiceContract
{
    public interface IAssetsManagerBase
    {
        Task<FileUploadResponse> UploadAsync(FileUploadRequest request);

        GenericResponse<string> GetUrl(GenericRequest<string> request);

        void SetupSubDirectory(GenericRequest<string> request);
    }
}
