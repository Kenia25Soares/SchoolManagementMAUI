using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Services.Interface;
using SchoolManagementMAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IUserSession _userSession;
        private readonly IAlertsService _alertsService;

        [ObservableProperty]
        private string userName = "User";

        [ObservableProperty]
        private string userEmail = "";

        [ObservableProperty]
        private bool isLoggedIn = false;

        [ObservableProperty]
        private string? userProfileImageUrl;

        [ObservableProperty]
        private int unreadAlertsCount = 0;

        [ObservableProperty]
        private bool hasUnreadAlerts = false;

        public DashboardViewModel(IUserSession userSession, IAlertsService alertsService)
        {
            _userSession = userSession;
            _alertsService = alertsService;
            LoadUserInfo();
        }

        public async void RefreshUserInfo()
        {
            LoadUserInfo();
            await LoadUnreadAlertsCountAsync();
        }

        private void LoadUserInfo()
        {
            if (_userSession.CurrentUser != null)
            {
                UserName = _userSession.CurrentUser.FullName ?? "User";
                UserEmail = _userSession.CurrentUser.Email ?? "";
                // Usar a URL 
                UserProfileImageUrl = !string.IsNullOrEmpty(_userSession.CurrentUser.ProfilePictureFullUrl)
                    ? _userSession.CurrentUser.ProfilePictureFullUrl
                    : _userSession.CurrentUser.ProfilePictureUrl;

                IsLoggedIn = true;
            }
            else
            {
                UserName = "User";
                UserEmail = "";
                UserProfileImageUrl = null;
                IsLoggedIn = false;
            }
        }

        [RelayCommand]
        private async Task OpenGradesAsync()
        {
            // Navigate to the new Student Overview page instead of direct grades
            var services = IPlatformApplication.Current?.Services;
            var overviewPage = services?.GetService<Views.StudentOverviewPage>();
            if (overviewPage != null)
            {
                await Shell.Current.Navigation.PushAsync(overviewPage);
            }
        }

        [RelayCommand]
        private async Task OpenEnrollmentRequestsAsync()
        {
            var services = IPlatformApplication.Current?.Services;
            var enrollmentPage = services?.GetService<EnrollmentRequestsPage>();
            if (enrollmentPage != null)
            {
                await Shell.Current.Navigation.PushAsync(enrollmentPage);
            }
        }

        [RelayCommand]
        private async Task OpenSubjectsAsync()
        {
            await Shell.Current.GoToAsync("//subject-list");
        }

        [RelayCommand]
        private async Task OpenProfileAsync()
        {
            await Shell.Current.GoToAsync("//profile");
        }

        [RelayCommand]
        private async Task OpenPublicCoursesAsync()
        {
            await Shell.Current.GoToAsync("//public-courses");
        }

        [RelayCommand]
        private async Task OpenPublicClassesAsync()
        {
            await Shell.Current.GoToAsync("//public-classes");
        }


        [RelayCommand]
        private async Task OpenAlertsAsync()
        {
            await Shell.Current.GoToAsync("//alerts");
        }

        private async Task LoadUnreadAlertsCountAsync()
        {
            if (_userSession.CurrentUser == null || string.IsNullOrEmpty(_userSession.CurrentUser.Token))
                return;

            try
            {
                var response = await _alertsService.GetUnreadCountAsync(
                    _userSession.CurrentUser.Id,
                    _userSession.CurrentUser.Token);

                if (response?.Success == true)
                {
                    UnreadAlertsCount = response.UnreadCount;
                    HasUnreadAlerts = UnreadAlertsCount > 0;
                }
            }
            catch
            {
                // Silently handle errors for unread count
            }
        }
    }
}

