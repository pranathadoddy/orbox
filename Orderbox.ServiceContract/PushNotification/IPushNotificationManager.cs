using Framework.ServiceContract.Response;
using Orderbox.ServiceContract.PushNotification.Request;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.PushNotification
{
    public interface IPushNotificationManager
    {
        Task<GenericResponse<bool>> PushMessageAsync(NotificationRequest request);
    }
}
