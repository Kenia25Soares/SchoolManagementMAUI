using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SchoolManagementMAUI.Services;
using SchoolManagementMAUI.Services.Interface;
using SchoolManagementMAUI.ViewModels;
using SchoolManagementMAUI.Views;


namespace SchoolManagementMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // Adiciona o CommunityToolkit
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            // Serviços e ViewModels 


            builder.Services.AddSingleton<IUserSession, UserSession>();

            builder.Services.AddSingleton<IAuthService, ApiAuthService>();
            builder.Services.AddSingleton<IGradesService, ApiGradesService>();


            // ViewModels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<GradesViewModel>();
           


            // Pages
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<GradesPage>();


            // Shell
            builder.Services.AddSingleton<AppShell>();


            // App
            builder.Services.AddSingleton<App>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
