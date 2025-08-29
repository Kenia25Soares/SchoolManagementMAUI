using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Services.Interface;
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

        [ObservableProperty]
        private string userName = "User";

        [ObservableProperty]
        private string userEmail = "";

        [ObservableProperty]
        private bool isLoggedIn = false;

        [ObservableProperty]
        private string? userProfileImageUrl;

        public DashboardViewModel(IUserSession userSession)
        {
            _userSession = userSession;
            LoadUserInfo();
        }

        public void RefreshUserInfo()
        {
            LoadUserInfo();
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
            await Shell.Current.GoToAsync("//grades");
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





        //[RelayCommand]
        //private async Task OpenCoursesAsync()
        //{
        //    await Shell.Current.GoToAsync("//courses");
        //}

        //[RelayCommand]
        //private async Task OpenProfileAsync()
        //{
        //    await Shell.Current.GoToAsync("//profile");
        //}

        //[RelayCommand]
        //private async Task OpenAboutAsync()
        //{
        //    await Shell.Current.GoToAsync("//about");
        //}
    }
}

