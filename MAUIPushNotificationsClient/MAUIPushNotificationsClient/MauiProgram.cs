using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Bundled.Platforms.Android;
using Plugin.Firebase.Crashlytics;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MAUIPushNotificationsClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .BuildConfigurations()
                .RegisterFirebaseServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();

            var app = builder.Build();
            Services = app.Services;
            return app;
        }

        public static IServiceProvider Services { get; private set; }

        private static MauiAppBuilder BuildConfigurations(this MauiAppBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var streamConfiguration = assembly.GetManifestResourceStream("MAUIPushNotificationsClient.app.settings.json");
            var config = new ConfigurationBuilder().AddJsonStream(streamConfiguration).Build();
            builder.Configuration.AddConfiguration(config);
            return builder;
        }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddAndroid(android =>
                {
                    android.OnCreate((activity, state) =>
                    {
                        Firebase.FirebaseApp.InitializeApp(activity);
                        CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings()
                        );
                        CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
                    });
                });
            });
            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(
                isAuthEnabled: true,
                isCloudMessagingEnabled: true,
                isAnalyticsEnabled: true);
        }
    }
}
