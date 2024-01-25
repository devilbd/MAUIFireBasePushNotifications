using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using PushnotificationsBack.DataTransferObjects;

namespace PushnotificationsBack.Services.FirebaseManager
{
    public class FireBaseManager : IFirebaseManager
    {
        public string Token { get; set; }
        public FirebaseApp App { get; protected set; }
        public FireBaseManager() {
            App = FirebaseApp.DefaultInstance;
            if (App == null)
            {
                App = FirebaseAdmin.FirebaseApp.Create(new AppOptions
                {
                    Credential = GetCredential().Result
                });
            }
        }

        public async Task SendMessage(MessageDTO messageDTO)
        {
            var messaging = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(App);
            var message = new Message()
            {
                Token = Token,
                Notification = new Notification { Title = messageDTO.Title, Body = messageDTO.Body },
                Data = new Dictionary<string, string> { { "greating", "hello" } },
                Android = new AndroidConfig { Priority = Priority.Normal },
                Apns = new ApnsConfig { Headers = new Dictionary<string, string> { { "apns-priority", "5" } } }
            };
            var response = await messaging.SendAsync(message);
        }

        private async Task<GoogleCredential> GetCredential()
        {
            return await GoogleCredential.FromFileAsync("firebase-adminsdk.json", CancellationToken.None);
        }
    }
}
