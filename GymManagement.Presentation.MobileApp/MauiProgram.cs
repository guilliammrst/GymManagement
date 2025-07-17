using GymManagement.Presentation.MobileApp.ApiClients;
using GymManagement.Presentation.MobileApp.Pages;
using GymManagement.Presentation.MobileApp.Services;
using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Environments;
using Microsoft.Extensions.Logging;

namespace GymManagement.Presentation.MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    // Ajout de plus de variantes de polices pour un design plus riche
                    fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                    fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient<GymApiClient>(client =>
            {
                client.BaseAddress = new(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariables.GymApiUrl));
            });

            builder.Services.AddHttpClient<AuthApiClient>(client =>
            {
                client.BaseAddress = new(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariables.AuthApiUrl));
            });

            builder.Services.AddSingleton<AuthenticatedUser>();
            builder.Services.AddSingleton<CoachingFlowData>();
            builder.Services.AddSingleton<SubscriptionFlowData>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddScoped<IPreferencesService, PreferencesService>();
            builder.Services.AddScoped<IApiClientHelper, ApiClientHelper>();

            builder.Services.RegisterPages();

            return builder.Build();
        }
    }
}
