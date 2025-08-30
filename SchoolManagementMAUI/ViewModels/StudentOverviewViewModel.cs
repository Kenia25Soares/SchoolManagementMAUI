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
    public partial class StudentOverviewViewModel : ObservableObject
    {
        private readonly IProfileService _profileService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string? studentName;

        [ObservableProperty]
        private string? studentEmail;

        [ObservableProperty]
        private string? studentClass;

        [ObservableProperty]
        private string? courseName;

        [ObservableProperty]
        private string? academicYear;

        [ObservableProperty]
        private bool isClassClosed;

        [ObservableProperty]
        private string? profilePictureUrl;

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        public StudentOverviewViewModel(IProfileService profileService, IUserSession userSession)
        {
            _profileService = profileService;
            _userSession = userSession;
        }

        [RelayCommand]
        public async Task LoadStudentInfoAsync()
        {
            if (!_userSession.IsLoggedIn)
            {
                Message = "You need to be authenticated.";
                return;
            }

            try
            {
                IsBusy = true;

                // Use session data as primary source since it's readily available
                if (_userSession.CurrentUser != null)
                {
                    StudentName = _userSession.CurrentUser.FullName;
                    StudentEmail = _userSession.CurrentUser.Email;
                    ProfilePictureUrl = _userSession.CurrentUser.ProfilePictureFullUrl;

                    // Try to get additional profile info
                    var profile = await _profileService.GetStudentProfileFullAsync(_userSession.CurrentUser.Token ?? "");
                    if (profile != null)
                    {
                        // Use profile data if available (might have more recent info)
                        StudentName = profile.FullName ?? StudentName;
                        StudentEmail = profile.Email ?? StudentEmail;
                        ProfilePictureUrl = profile.ProfilePictureFullUrl ?? ProfilePictureUrl;
                    }

                    // For class info, we'll need to get it from grades or use defaults
                    StudentClass = "Academic Class"; // Default - could be enhanced later
                    CourseName = "Computer Science"; // Default - could be enhanced later  
                    AcademicYear = "2024/2025";
                    IsClassClosed = false; // Default to active class

                    Message = string.Empty;
                }
                else
                {
                    Message = "User session not available.";

                }
            }
            catch (Exception )
            {
                Message = "Error loading student information.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ViewClassDetailsAsync()
        {
            var services = IPlatformApplication.Current?.Services;
            var classDetailPage = services?.GetService<Views.ClassGradesPage>();
            if (classDetailPage != null)
            {
                await Shell.Current.Navigation.PushAsync(classDetailPage);
            }
        }

        [RelayCommand]
        private async Task ViewAllGradesAsync()
        {
            // Navigate to the current  grades list as fallback
            var services = IPlatformApplication.Current?.Services;
            var gradesPage = services?.GetService<Views.GradesPage>();
            if (gradesPage != null)
            {
                await Shell.Current.Navigation.PushAsync(gradesPage);
            }
        }
    }
}
