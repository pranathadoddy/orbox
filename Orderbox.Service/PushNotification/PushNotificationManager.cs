using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Framework.ServiceContract.Response;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Orderbox.ServiceContract.PushNotification;
using Orderbox.ServiceContract.PushNotification.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Orderbox.Service.PushNotification
{
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly IConfiguration _configuration;

        public PushNotificationManager(IConfiguration configuration)
        {
            this._configuration = configuration;

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orderbox-web-firebase-adminsdk.json"))
            });
        }

        public async Task<GenericResponse<bool>> PushMessageAsync(NotificationRequest request)
        {
            var response = new GenericResponse<bool>();

            try
            {
                var logoUrl = this._configuration.GetValue<string>("Application:Logo");
                var messsage = new Message
                {
                    Token = request.Token,
                    Data = new Dictionary<string, string>
                    {
                        { "Title", request.Title },
                        { "Body", request.Body },
                        { "Icon", logoUrl },
                        { "RedirectUrl", request.RedirectUrl }
                    }
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                await messaging.SendAsync(messsage);

                response.Data = true;
            }
            catch (Exception e)
            {
                response.Data = false;
                response.AddErrorMessage(e.Message);
            }

            return response;
        }
    }
}
