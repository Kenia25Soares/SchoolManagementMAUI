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
    public partial class EnhancedSubjectDetailViewModel : ObservableObject
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

        // Grouped grades by type
        public ObservableCollection<GradeTypeGroup> GradesByType { get; } = new();
        public ObservableCollection<StudentAbsenceDetail> Absences { get; } = new();

        public EnhancedSubjectDetailViewModel(IGradesService gradesService, IUserSession userSession)
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

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(SubjectId))
            {
                Message = "Invalid parameters.";
                return;
            }

            try
            {
                IsBusy = true;
                var detail = await _gradesService.GetStudentSubjectDetailAsync(studentId, SubjectId ?? "0", token);

                if (detail == null)
                {
                    // Fallback: Get basic info from the summary list
                    var summaries = await _gradesService.GetStudentSubjectsSummaryAsync(studentId, token);
                    var matchingSummary = summaries?.FirstOrDefault(s => s.SubjectId == SubjectId || s.SubjectName == SubjectName);

                    if (matchingSummary != null)
                    {

                        // Create a basic detail from summary data
                        SubjectDetail = new StudentSubjectDetail
                        {
                            SubjectId = matchingSummary.SubjectId,
                            SubjectName = matchingSummary.SubjectName,
                            SubjectCode = matchingSummary.SubjectCode,
                            TeacherName = string.IsNullOrWhiteSpace(matchingSummary.TeacherName) || matchingSummary.TeacherName == "Professor"
                                ? null : matchingSummary.TeacherName, // Hide generic teacher name
                            WeightedAverage = matchingSummary.WeightedAverage,
                            TotalAbsences = matchingSummary.TotalAbsences,
                            AllowedAbsences = matchingSummary.AllowedAbsences,
                            FailedDueToAbsences = matchingSummary.FailedDueToAbsences,
                            Status = matchingSummary.Status,
                            StatusDescription = matchingSummary.StatusDescription,
                            GradeDetails = new List<StudentGradeDetail>(), // Will be empty until API provides details
                            AbsenceDetails = new List<StudentAbsenceDetail>() // Will be empty until API provides details
                        };

                        SubjectName = matchingSummary.SubjectName;
                        Message = "Using fallback data - API detail endpoint may not be working.";
                    }
                    else
                    {
                        Message = "Subject details not found.";
                        return;
                    }
                }
                else
                {
                    SubjectDetail = detail;
                    SubjectName = detail.SubjectName;
                }

                // Process grades and absences if we have detail data
                if (SubjectDetail != null)
                {
                    // Group grades by type
                    GradesByType.Clear();
                    if (SubjectDetail.GradeDetails != null && SubjectDetail.GradeDetails.Count > 0)
                    {
                        var groupedGrades = SubjectDetail.GradeDetails
                            .GroupBy(g => g.EvaluationType ?? "Other")
                            .OrderBy(g => GetTypeOrder(g.Key));

                        foreach (var group in groupedGrades)
                        {
                            var typeGroup = new GradeTypeGroup
                            {
                                TypeName = group.Key,
                                TypeDisplayName = GetTypeDisplayName(group.Key),
                                Grades = new ObservableCollection<StudentGradeDetail>(group.OrderByDescending(g => g.Date)),
                                Average = group.Average(g => g.Value),
                                Count = group.Count()
                            };
                            GradesByType.Add(typeGroup);
                        }

                    }

                    // Load absences
                    Absences.Clear();
                    if (SubjectDetail.AbsenceDetails != null)
                    {
                        foreach (var absence in SubjectDetail.AbsenceDetails.OrderByDescending(a => a.Date))
                        {
                            Absences.Add(absence);
                        }
                    }
                }

                // Clear message if we have real data, keep fallback message if using summary
                if (detail != null)
                {
                    Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message = "Error loading subject details.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static int GetTypeOrder(string type)
        {
            return type?.ToLower() switch
            {
                "assignment" => 1,
                "test" => 2,
                "exam" => 3,
                _ => 4
            };
        }

        private static string GetTypeDisplayName(string type)
        {
            return type?.ToLower() switch
            {
                "assignment" => "Assignments",
                "test" => "Tests",
                "exam" => "Exams",
                _ => type ?? "Other"
            };
        }


    }
}