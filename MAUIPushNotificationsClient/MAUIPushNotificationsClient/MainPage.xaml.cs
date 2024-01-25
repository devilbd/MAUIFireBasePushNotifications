using Firebase.Messaging;
using Plugin.Firebase.CloudMessaging;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using static Android.Gms.Common.Apis.Api;
using System.Text;
using System.Text.Json;

namespace MAUIPushNotificationsClient
{
    public partial class MainPage : ContentPage
    {
        public string Token { get; set; }
        FirebaseAdmin.FirebaseApp app;

        private readonly IConfiguration _Configuration;

        private HttpClient _HttpClient = new HttpClient();

        public MainPage(IConfiguration configuration)
        {
            _Configuration = configuration;
            InitializeComponent();
            AutoUpdateToken();
        }

        private async void OnGetToken(object sender, EventArgs e)
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                Token = token;
                await DisplayAlert("FCM token", token, "OK");
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AutoUpdateToken()
        {
            await Task.Delay(5000);
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

            var pushnotificationsEndPoint = _Configuration["Settings:PushnotificationsAPIEndpoint"].ToString();
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(token),
                Encoding.UTF8,
            "application/json");
            using HttpResponseMessage response = await _HttpClient.PostAsync(pushnotificationsEndPoint + "/api/update-token", jsonContent);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            await AutoUpdateToken();
        }

        private async void OnSendNotification(object sender, EventArgs e)
        {
            try
            {
                if (app == null)
                {
                    app = FirebaseAdmin.FirebaseApp.Create(new AppOptions
                    {
                        Credential = await GetCredential()
                    });
                }

                var messaging = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);
                var message = new Message()
                {
                    Token = Token,
                    Notification = new Notification { Title = "Hello world!", Body = "It's a message for Android with MAUI" },
                    Data = new Dictionary<string, string> { { "greating", "hello" } },
                    Android = new AndroidConfig { Priority = Priority.Normal },
                    Apns = new ApnsConfig { Headers = new Dictionary<string, string> { { "apns-priority", "5" } } }
                };
                var response = await messaging.SendAsync(message);
                await DisplayAlert("Response", response, "OK");
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<GoogleCredential> GetCredential()
        {
            var path = await FileSystem.OpenAppPackageFileAsync("firebase-adminsdk.json");
            return GoogleCredential.FromStream(path);
        }
    }

}
