using Framework.Service;
using Microsoft.Extensions.Configuration;
using Orderbox.Core.Resources.Email;
using Orderbox.ServiceContract.Email;

namespace Orderbox.Service.Email
{
    public class AgentInvitationEmailManager : AwsSesEmailManagerBase, IAgentInvitationEmailManager
    {
        public AgentInvitationEmailManager(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string TemplateKeyName => "Orderbox_AgentInvitationEmail";

        protected override string Subject => EmailSubjectResource.AgentInvitationEmail;
    }
}
