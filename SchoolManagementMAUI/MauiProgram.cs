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
            builder.Services.AddSingleton<IProfileService, ApiProfileService>();
            builder.Services.AddSingleton<IPublicCatalogService, ApiPublicCatalogService>();
            builder.Services.AddSingleton<IEnrollmentService, ApiEnrollmentService>();
            builder.Services.AddSingleton<IAlertsService, ApiAlertsService>();

            // ViewModels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<GradesViewModel>();
            builder.Services.AddSingleton<UpdatePasswordViewModel>();
            builder.Services.AddSingleton<PasswordManagementViewModel>();
            builder.Services.AddSingleton<SubjectListViewModel>();
            builder.Services.AddSingleton<SubjectGradeViewModel>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<CoursesListViewModel>();
            builder.Services.AddSingleton<ClassesListViewModel>();
            builder.Services.AddSingleton<SubjectsListViewModel>();
            builder.Services.AddSingleton<CourseDetailViewModel>();
            builder.Services.AddSingleton<ClassDetailViewModel>();
            builder.Services.AddSingleton<SubjectDetailViewModel>();
            builder.Services.AddTransient<StudentOverviewViewModel>();
            builder.Services.AddTransient<ClassGradesViewModel>();
            builder.Services.AddTransient<EnhancedSubjectDetailViewModel>();
            builder.Services.AddTransient<EnrollmentRequestsViewModel>();
            builder.Services.AddTransient<AvailableSubjectsViewModel>();
            builder.Services.AddTransient<CreateEnrollmentRequestViewModel>();
            builder.Services.AddTransient<MyEnrollmentRequestsViewModel>();
            builder.Services.AddSingleton<AlertsViewModel>();




            // Pages
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<GradesPage>();
            builder.Services.AddSingleton<UpdatePasswordPage>();
            builder.Services.AddSingleton<PasswordManagementPage>();
            builder.Services.AddSingleton<SubjectListPage>();
            builder.Services.AddSingleton<SubjectGradePage>();
            builder.Services.AddSingleton<PublicHomePage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddSingleton<CoursesListPage>();
            builder.Services.AddSingleton<ClassesListPage>();
            builder.Services.AddSingleton<SubjectsListPage>();
            builder.Services.AddSingleton<CourseDetailPage>();
            builder.Services.AddSingleton<ClassDetailPage>();
            builder.Services.AddSingleton<SubjectDetailPage>();
            builder.Services.AddTransient<StudentOverviewPage>();
            builder.Services.AddTransient<ClassGradesPage>();
            builder.Services.AddTransient<EnhancedSubjectDetailPage>();
            builder.Services.AddTransient<EnrollmentRequestsPage>();
            builder.Services.AddTransient<AvailableSubjectsPage>();
            builder.Services.AddTransient<CreateEnrollmentRequestPage>();
            builder.Services.AddTransient<MyEnrollmentRequestsPage>();
            builder.Services.AddTransient<AlertsPage>();


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
