using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Framework.Core;
using Framework.Core.Resources;
using Framework.ServiceContract;
using Framework.ServiceContract.FileUpload.Request;
using Framework.ServiceContract.FileUpload.Response;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.Extensions.Configuration;
using MimeTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Service.FileUpload
{
    public abstract class AwsS3AssetsManagerBase : IAssetsManagerBase
    {
        private IConfiguration _configuration;

        private string _provider;
        private string _bucketName;
        private string _mainDirectory;
        private string _endpoint;
        private string _region;
        private string _cdn;
        private string _doAccessKey;
        private string _doAccessSecret;

        protected abstract string BaseDirectory { get; }
        private string FilePath { get; set; }

        public AwsS3AssetsManagerBase(IConfiguration configuration)
        {
            this._configuration = configuration;

            this._provider = this._configuration.GetValue<string>("S3CompatibleStore:Provider");
            this._bucketName = this._configuration.GetValue<string>("S3CompatibleStore:Bucket");
            this._mainDirectory = this._configuration.GetValue<string>("S3CompatibleStore:MainDirectory");
            this._endpoint = this._configuration.GetValue<string>("S3CompatibleStore:Endpoint");
            this._region = this._configuration.GetValue<string>("S3CompatibleStore:Region");
            this._cdn = this._configuration.GetValue<string>("S3CompatibleStore:Cdn");
            this._doAccessKey = this._configuration.GetValue<string>("S3CompatibleStore:Key");
            this._doAccessSecret = this._configuration.GetValue<string>("S3CompatibleStore:Secret");

            this.FilePath = this.BaseDirectory;
        }

        public async Task<FileUploadResponse> UploadAsync(FileUploadRequest request)
        {
            RegionEndpoint regionEndPoint = null;
            if (!string.IsNullOrEmpty(this._region))
            {
                regionEndPoint = RegionEndpoint.GetBySystemName(this._region);
            }

            var response = new FileUploadResponse();

            using (var client = 
                new AmazonS3Client(
                    this._doAccessKey, 
                    this._doAccessSecret, 
                    new AmazonS3Config { 
                        ServiceURL = this._endpoint, 
                        RegionEndpoint = regionEndPoint 
                    }
                )
            )
            {
                try
                {
                    var fileExtension = Path.GetExtension(request.FileName);
                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        fileExtension = MimeTypeMap.GetExtension(request.MimeType);
                    }
                    var serverFileName = $"{Guid.NewGuid().ToString().ToLower()}{fileExtension}";

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = $"{this._bucketName}/{this._mainDirectory}/{this.FilePath}",
                        InputStream = request.FileStream,
                        Key = serverFileName
                    };

                    if (this._provider == FrameworkCoreConstant.S3CompatibleStoreProvider.DoSpace)
                    {
                        uploadRequest.CannedACL = S3CannedACL.PublicRead;
                    }

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    response = new FileUploadResponse
                    {
                        Name = request.FileName,
                        ServerFileName = serverFileName,
                        Url = $"{this._cdn}/{this._mainDirectory}/{this.FilePath}/{serverFileName}",
                        MimeType = request.MimeType,
                        Size = request.FileSize
                    };

                    return response;
                }
                catch (Exception e)
                {
                    response.AddErrorMessage(e.Message);
                    return response;
                }
            }
        }

        public GenericResponse<string> GetUrl(GenericRequest<string> request)
        {
            var response = new GenericResponse<string>();
            if (string.IsNullOrEmpty(request.Data))
            {
                response.Data = string.Empty;
                response.AddErrorMessage(GeneralResource.Assets_NoFileNameSpecified);
                return response;
            }

            response.Data = $"{this._cdn}/{this._mainDirectory}/{this.FilePath}/{request.Data}";
            return response;
        }

        public virtual void SetupSubDirectory(GenericRequest<string> request)
        {
            this.FilePath = $"{this.BaseDirectory}/{request.Data}";
        }
    }
}
