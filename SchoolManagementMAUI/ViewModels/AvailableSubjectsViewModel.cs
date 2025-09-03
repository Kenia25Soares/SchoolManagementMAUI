using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using SchoolManagementMAUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class AvailableSubjectsViewModel : ObservableObject
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private ObservableCollection<AvailableSubjectSummary> availableSubjects = new();

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy = false;

        public AvailableSubjectsViewModel(IEnrollmentService enrollmentService, IUserSession userSession)
        {
            _enrollmentService = enrollmentService;
            _userSession = userSession;
        }

        [RelayCommand]
        public async Task LoadAvailableSubjectsAsync()
        {
            var studentId = _userSession.CurrentUser?.Id;
            var token = _userSession.CurrentUser?.Token;

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(token))
            {
                Message = "Authentication required.";
                return;
            }

            try
            {
                IsBusy = true;
                var subjects = await _enrollmentService.GetAvailableSubjectsAsync(studentId, token);

                AvailableSubjects.Clear();
                foreach (var subject in subjects)
                {
                    AvailableSubjects.Add(subject);
                }

                if (subjects.Count == 0)
                {
                    Message = "No additional subjects available for enrollment.";
                }
                else
                {
                    Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message = "Error loading available subjects.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RequestEnrollmentAsync(AvailableSubjectSummary subject)
        {
            if (subject == null) return;

            var services = IPlatformApplication.Current?.Services;
            var page = services?.GetService<CreateEnrollmentRequestPage>();
            if (page != null)
            {
                page.Initialize(subject);
                await Shell.Current.Navigation.PushAsync(page);
            }
        }
    }
}
