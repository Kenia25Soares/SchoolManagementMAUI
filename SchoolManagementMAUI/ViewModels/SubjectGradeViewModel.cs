using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System.Collections.ObjectModel;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class SubjectGradeViewModel : ObservableObject
    {
        private readonly IGradesService _gradesService;
        private readonly IUserSession _userSession;

        public SubjectGradeViewModel(IGradesService gradesService, IUserSession userSession)
        {
            _gradesService = gradesService;
            _userSession = userSession;
        }

        [ObservableProperty]
        private SubjectGrade subjectGrade;

        [ObservableProperty]
        private string subjectName;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasData = false;

        [ObservableProperty]
        private ObservableCollection<GradeDetail> gradeDetails = new();

        [ObservableProperty]
        private ObservableCollection<AbsenceDetail> absenceDetails = new();

        public void SetSubject(string subjectCode, string subjectName)
        {
            SubjectName = subjectName;
            LoadSubjectGradeAsync(subjectCode);
        }

        [RelayCommand]
        private async Task LoadSubjectGradeAsync(string subjectCode)
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

                var response = await _gradesService.GetSubjectGradeAsync(studentId, subjectCode, token);

                if (response == null)
                {
                    Message = "No data found for this subject.";
                    HasData = false;
                    return;
                }

                SubjectGrade = response;
                GradeDetails = new ObservableCollection<GradeDetail>(response.GradeDetails);
                AbsenceDetails = new ObservableCollection<AbsenceDetail>(response.AbsenceDetails);
                HasData = true;
                Message = string.Empty;
            }
            catch (Exception ex)
            {
                Message = "Error loading subject grade.";
                HasData = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
