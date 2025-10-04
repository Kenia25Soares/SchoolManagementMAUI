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
    public partial class GradesViewModel : ObservableObject
    {
        private readonly IGradesService _gradesService;
        private readonly IUserSession _userSession;

        public GradesViewModel(IGradesService gradesService, IUserSession userSession)
        {
            _gradesService = gradesService;
            _userSession = userSession;
        }

        [ObservableProperty]
        private ObservableCollection<StudentSubjectSummary> subjectSummaries = new();

        [ObservableProperty]
        private string studentName;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool isBusy;

        [RelayCommand]
        public async Task LoadGradesAsync()
        {
            if (!_userSession.IsLoggedIn)
            {
                Message = "You need to be authenticated.";
                return;
            }

            var studentId = _userSession.CurrentUser?.Id;
            var token = _userSession.CurrentUser?.Token;

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(token))
            {
                Message = "Invalid token or ID.";
                return;
            }

            try
            {
                IsBusy = true;

                var summaries = await _gradesService.GetStudentSubjectsSummaryAsync(studentId, token);

                if (summaries != null)
                {
                    for (int i = 0; i < summaries.Count; i++)
                    {
                        var summary = summaries[i];

                    }
                }
                if (summaries == null || summaries.Count == 0)
                {
                    Message = "No subjects found. Please check if you have grades registered or contact support.";
                    SubjectSummaries = new ObservableCollection<StudentSubjectSummary>();
                    return;
                }

                SubjectSummaries = new ObservableCollection<StudentSubjectSummary>(summaries);
                StudentName = _userSession.CurrentUser.FullName;
                Message = string.Empty;
            }
            catch (Exception ex)
            {
                Message = "Error loading grades.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task OpenSubjectDetailAsync(StudentSubjectSummary subject)
        {
            if (subject == null) return;

            var services = IPlatformApplication.Current?.Services;
            var detailPage = services?.GetService<SubjectDetailPage>();
            if (detailPage != null)
            {
                detailPage.Initialize(subject.SubjectId, subject.SubjectName ?? "Subject");
                await Shell.Current.Navigation.PushAsync(detailPage);
            }
        }
    }
}