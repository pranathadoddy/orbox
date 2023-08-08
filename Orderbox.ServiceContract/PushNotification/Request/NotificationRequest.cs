namespace Orderbox.ServiceContract.PushNotification.Request
{
    public class NotificationRequest
    {
        public string Token { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string RedirectUrl { get; set; }
    }
}
