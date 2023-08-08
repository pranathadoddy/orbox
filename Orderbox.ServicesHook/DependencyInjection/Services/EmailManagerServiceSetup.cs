using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Email;
using Orderbox.ServiceContract.Email;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class EmailManagerServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IAgentInvitationEmailManager, AgentInvitationEmailManager>();
            service.AddScoped<IResetPasswordEmailManager, ResetPasswordEmailManager>();
            service.AddScoped<IVerificationEmailManager, VerificationEmailManager>();            
        }
    }
}
