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
    public partial class SubjectDetailViewModel : ObservableObject
    {
        private readonly IGradesService _gradesService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string? subjectId;

        [ObservableProperty]
        private string? subjectName;

        [ObservableProperty]
        private StudentSubjectDetail? subjectDetail;

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        public ObservableCollection<StudentGradeDetail> Grades { get; } = new();
        public ObservableCollection<StudentAbsenceDetail> Absences { get; } = new();

        public SubjectDetailViewModel(IGradesService gradesService, IUserSession userSession)
        {
            _gradesService = gradesService;
            _userSession = userSession;
        }

        public void Initialize(string? subjectId, string? subjectName = null)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
        }

        [RelayCommand]
        public async Task LoadDetailAsync()
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
                var detail = await _gradesService.GetStudentSubjectDetailAsync(studentId, SubjectId ?? "0", token);

                if (detail == null)
                {
                    Message = "Subject details not found.";
                    return;
                }

                SubjectDetail = detail;
                SubjectName = detail.SubjectName;

                Grades.Clear();
                if (detail.GradeDetails != null)
                {
                    foreach (var grade in detail.GradeDetails)
                    {
                        Grades.Add(grade);
                    }
                }

                Absences.Clear();
                if (detail.AbsenceDetails != null)
                {
                    foreach (var absence in detail.AbsenceDetails)
                    {
                        Absences.Add(absence);
                    }
                }

                Message = string.Empty;
            }
            catch (Exception)
            {
                Message = "Error loading subject details.";

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
