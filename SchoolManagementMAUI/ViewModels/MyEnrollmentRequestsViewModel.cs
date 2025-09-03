using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class MyEnrollmentRequestsViewModel : ObservableObject
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private ObservableCollection<EnrollmentRequestSummary> enrollmentRequests = new();

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy = false;

        public MyEnrollmentRequestsViewModel(IEnrollmentService enrollmentService, IUserSession userSession)
        {
            _enrollmentService = enrollmentService;
            _userSession = userSession;
        }

        [RelayCommand]
        public async Task LoadMyRequestsAsync()
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
                var requests = await _enrollmentService.GetMyRequestsAsync(studentId, token);

                EnrollmentRequests.Clear();
                foreach (var request in requests.OrderByDescending(r => r.RequestDate))
                {
                    EnrollmentRequests.Add(request);
                }

                if (requests.Count == 0)
                {
                    Message = "You haven't made any enrollment requests yet.";
                }
                else
                {
                    Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message = "Error loading enrollment requests.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ViewRequestDetailsAsync(EnrollmentRequestSummary request)
        {
            if (request == null) return;

            var details = $"Subject: {request.SubjectName}\n" +
                         $"Code: {request.SubjectCode}\n" +
                         $"Status: {request.Status}\n" +
                         $"Request Date: {request.RequestDate:dd/MM/yyyy HH:mm}\n\n" +
                         $"Your Description:\n{request.Description}";

            if (!string.IsNullOrEmpty(request.ResponseMessage))
            {
                details += $"\n\nStaff Response:\n{request.ResponseMessage}";
            }

            if (request.ProcessedDate.HasValue)
            {
                details += $"\n\nProcessed: {request.ProcessedDate:dd/MM/yyyy HH:mm}";
                if (!string.IsNullOrEmpty(request.ProcessedByName))
                {
                    details += $"\nBy: {request.ProcessedByName}";
                }
            }

            await Application.Current!.MainPage!.DisplayAlert(
                $"Request Details - {request.Status}",
                details,
                "OK");
        }
    }
}
