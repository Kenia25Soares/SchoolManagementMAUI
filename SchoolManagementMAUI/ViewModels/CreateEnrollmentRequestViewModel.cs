using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class CreateEnrollmentRequestViewModel : ObservableObject
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private AvailableSubjectSummary? selectedSubject;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private int characterCount = 0;

        public CreateEnrollmentRequestViewModel(IEnrollmentService enrollmentService, IUserSession userSession)
        {
            _enrollmentService = enrollmentService;
            _userSession = userSession;
        }

        partial void OnDescriptionChanged(string value)
        {
            CharacterCount = value?.Length ?? 0;
        }

        public void SetSelectedSubject(AvailableSubjectSummary subject)
        {
            SelectedSubject = subject;
        }

        [RelayCommand]
        private async Task SubmitRequestAsync()
        {
            if (SelectedSubject == null)
            {
                Message = "No subject selected.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                Message = "Please provide a description for your enrollment request.";
                return;
            }

            if (Description.Length < 10)
            {
                Message = "Description must be at least 10 characters long.";
                return;
            }

            if (Description.Length > 500)
            {
                Message = "Description cannot exceed 500 characters.";
                return;
            }

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
                Message = string.Empty;

                var request = new CreateEnrollmentRequest
                {
                    StudentId = studentId,
                    SubjectId = SelectedSubject.SubjectId,
                    Description = Description.Trim()
                };

                var success = await _enrollmentService.CreateEnrollmentRequestAsync(request, token);

                if (success)
                {
                    await Application.Current!.MainPage!.DisplayAlert(
                        "Success",
                        "Your enrollment request has been submitted successfully. You will be notified when it's reviewed.",
                        "OK");

                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    Message = "Failed to submit enrollment request. Please try again.";
                }
            }
            catch (Exception ex)
            {
                Message = "An error occurred while submitting your request.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
