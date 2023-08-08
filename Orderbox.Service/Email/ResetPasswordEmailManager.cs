using Framework.Service;
using Microsoft.Extensions.Configuration;
using Orderbox.Core.Resources.Email;
using Orderbox.ServiceContract.Email;

namespace Orderbox.Service.Email
{
    public class ResetPasswordEmailManager : AwsSesEmailManagerBase, IResetPasswordEmailManager
    {
        public ResetPasswordEmailManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string TemplateKeyName => "Orderbox_ResetPasswordEmail";

        protected override string Subject => EmailSubjectResource.ResetPasswordEmail;
    }
}
