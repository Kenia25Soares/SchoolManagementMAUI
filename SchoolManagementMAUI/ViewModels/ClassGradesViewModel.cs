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
    public partial class ClassGradesViewModel : ObservableObject
    {
        private readonly IGradesService _gradesService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string? className;

        [ObservableProperty]
        private string? courseName;

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        public ObservableCollection<StudentSubjectSummary> ClassSubjects { get; } = new();

        public ClassGradesViewModel(IGradesService gradesService, IUserSession userSession)
        {
            _gradesService = gradesService;
            _userSession = userSession;
        }

        [RelayCommand]
        public async Task LoadClassSubjectsAsync()
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

                ClassSubjects.Clear();
                if (summaries != null && summaries.Count > 0)
                {
                    foreach (var summary in summaries)
                    {
                        ClassSubjects.Add(summary);
                    }
                    Message = string.Empty;
                }
                else
                {
                    Message = "No subjects found for this class.";
                }
            }
            catch (Exception ex)
            {
                Message = "Error loading class subjects.";
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
            var detailPage = services?.GetService<EnhancedSubjectDetailPage>();
            if (detailPage != null)
            {
                detailPage.Initialize(subject.SubjectId, subject.SubjectName);
                await Shell.Current.Navigation.PushAsync(detailPage);
            }
        }
    }
}
