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
    public partial class SubjectListViewModel : ObservableObject
    {
        private readonly IGradesService _gradesService;
        private readonly IUserSession _userSession;

        public SubjectListViewModel(IGradesService gradesService, IUserSession userSession)
        {
            _gradesService = gradesService;
            _userSession = userSession;
        }

        [ObservableProperty]
        private ObservableCollection<SubjectInfo> subjects = new();

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string studentName;

        [RelayCommand]
        public async Task LoadSubjectsAsync()
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
                IsLoading = true;
                Message = string.Empty;

                var response = await _gradesService.GetStudentSubjectsAsync(studentId, token);

                if (response == null || response.Count == 0)
                {
                    Message = "No subjects found.";
                    Subjects = new ObservableCollection<SubjectInfo>();
                    return;
                }

                Subjects = new ObservableCollection<SubjectInfo>(response);
                StudentName = _userSession.CurrentUser.FullName;
                Message = string.Empty;
            }
            catch (Exception ex)
            {
                Message = "Error loading subjects.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectSubjectAsync(SubjectInfo subject)
        {
            if (subject == null) return;

            var parameters = new Dictionary<string, object>
            {
                { "subjectCode", subject.SubjectCode },
                { "subjectName", subject.SubjectName }
            };

            await Shell.Current.GoToAsync("subject-grade", parameters);
        }
    }
}