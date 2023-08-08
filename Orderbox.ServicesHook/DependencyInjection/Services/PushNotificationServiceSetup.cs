using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.PushNotification;
using Orderbox.ServiceContract.PushNotification;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class PushNotificationServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddSingleton<IPushNotificationManager, PushNotificationManager>();
        }
    }
}
