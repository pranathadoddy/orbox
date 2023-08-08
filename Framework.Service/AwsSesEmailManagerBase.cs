using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Framework.ServiceContract;
using Framework.ServiceContract.Response;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Service
{
    public abstract class AwsSesEmailManagerBase : IAwsSesEmailManagerBase
    {
        public AwsSesEmailManagerBase(IConfiguration configuration)
        {
            Configuration = configuration;
            
            this._awsAccessKey = Configuration.GetValue<string>("EmailConfiguration:AWS.SES:AccessKey");
            this._awsSecretAccessKey = Configuration.GetValue<string>("EmailConfiguration:AWS.SES:SecretAccessKey");
            this._sender = Configuration.GetValue<string>("EmailConfiguration:GeneralSenderAddress");
        }

        #region Fields

        private string _sender;

        private List<Dictionary<string, string>> _recipients;

        private string _awsAccessKey;

        private string _awsSecretAccessKey;

        #endregion

        #region Properties

        protected IConfiguration Configuration { get; }

        protected abstract string TemplateKeyName { get; }

        protected abstract string Subject { get; }

        #endregion

        #region IEmailManagerBase Members

        public ICollection<Dictionary<string, string>> Recipients => 
            _recipients ?? (_recipients = new List<Dictionary<string, string>>());

        public string TemplateFilePath { get; set; }

        public async Task BulkSendAsync()
        {
            var destinations = new List<BulkEmailDestination>();
            foreach (var recipient in this.Recipients)
            {
                destinations.Add(new BulkEmailDestination
                {
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { recipient["ToAddress"] }
                    },
                    ReplacementTemplateData = recipient["ReplacementTemplateData"]
                });
            }

            using (var client = new AmazonSimpleEmailServiceClient(
                this._awsAccessKey, 
                this._awsSecretAccessKey, 
                Amazon.RegionEndpoint.APSoutheast1
            ))
            {
                var sendBulkTemplatedRequest = new SendBulkTemplatedEmailRequest
                {
                    Source = this._sender,
                    Template = this.TemplateKeyName,
                    DefaultTemplateData = "{\"key\":\"value\"}",
                    Destinations = destinations
                };

                var response = await client.SendBulkTemplatedEmailAsync(sendBulkTemplatedRequest);
            }
        }

        public virtual void Clear()
        {
            _recipients = null;
        }

        public async Task DeleteTemplateAsync()
        {
            using (var client = new AmazonSimpleEmailServiceClient(this._awsAccessKey, this._awsSecretAccessKey))
            {
                var deleteTemplateRequest = new DeleteTemplateRequest
                {
                    TemplateName = this.TemplateKeyName
                };

                await client.DeleteTemplateAsync(deleteTemplateRequest);
            }
        }

        public async Task SendAsync()
        {
            var destination = new Destination
            {
                ToAddresses = new List<string> { this.Recipients.First()["ToAddress"] }
            };
            var templateData = this.Recipients.First()["ReplacementTemplateData"];

            using (var client = new AmazonSimpleEmailServiceClient(
                this._awsAccessKey,
                this._awsSecretAccessKey,
                Amazon.RegionEndpoint.APSoutheast1
            ))
            {
                var sendTemplatedRequest = new SendTemplatedEmailRequest
                {
                    Source = this._sender,
                    Template = this.TemplateKeyName,
                    TemplateData = templateData,
                    Destination = destination
                };

                await client.SendTemplatedEmailAsync(sendTemplatedRequest);
            }
        }

        public async Task<BasicResponse> UploadTemplateAsync()
        {
            var response = new BasicResponse();

            var fileName = Configuration.GetValue<string>("EmailConfiguration:EmailTemplates:" + TemplateKeyName);
            var templateFileName = Path.Combine(TemplateFilePath, fileName);

            var templateContent = File.ReadAllText(templateFileName);

            using (var client = new AmazonSimpleEmailServiceClient(this._awsAccessKey, this._awsSecretAccessKey))
            {
                try
                {
                    var getTemplateRequest = new GetTemplateRequest
                    {
                        TemplateName = this.TemplateKeyName
                    };
                    await client.GetTemplateAsync(getTemplateRequest);
                    
                    response.AddErrorMessage("Template already exists.");
                    return response;
                }
                catch(TemplateDoesNotExistException)
                {
                    var createTemplateRequest = new CreateTemplateRequest
                    {
                        Template = new Template
                        {
                            SubjectPart = this.Subject,
                            TemplateName = this.TemplateKeyName,
                            HtmlPart = templateContent,
                            TextPart = templateContent
                        }
                    };
                    await client.CreateTemplateAsync(createTemplateRequest);

                    response.AddInfoMessage($"Successfully upload template with key: {TemplateKeyName}");
                    return response;
                }
                catch
                {
                    response.AddInfoMessage($"Network issue upon uploading: {TemplateKeyName}");
                    return response;
                }
            }
        }

        #endregion
    }
}
