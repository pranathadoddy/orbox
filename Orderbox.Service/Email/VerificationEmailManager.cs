using Framework.Service;
using Microsoft.Extensions.Configuration;
using Orderbox.Core.Resources.Email;
using Orderbox.ServiceContract.Email;

namespace Orderbox.Service.Email
{
    public class VerificationEmailManager : AwsSesEmailManagerBase, IVerificationEmailManager
    {
        public VerificationEmailManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string TemplateKeyName => "Orderbox_VerificationEmail";

        protected override string Subject => EmailSubjectResource.VerificationEmail;
    }
}
