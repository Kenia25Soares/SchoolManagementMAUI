using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SchoolManagementMAUI.Services
{
    public class ApiGradesService : IGradesService
    {
        private readonly HttpClient _client;
        private const string ApiBaseUrl = "https://10.0.2.2:7176/api"; 
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiGradesService()
        {
            _client = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });
        }

        public async Task<List<Grade>> GetGradesAsync(string studentId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{ApiBaseUrl}/GradesAPI/{studentId}";

                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Grade>();
                }

                var result = await response.Content
                    .ReadFromJsonAsync<GradesApiResponse>(JsonOptions);

                return result?.SubjectGrades ?? new List<Grade>();
            }
            catch (Exception )
            {
                return new List<Grade>();
            }
        }


        public async Task<List<StudentSubjectSummary>> GetStudentSubjectsSummaryAsync(string studentId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{ApiBaseUrl}/grades/{studentId}/mobile/subjects";
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<StudentSubjectsListResponse>(json, JsonOptions);

                    if (apiResponse?.Subjects != null && apiResponse.Subjects.Count > 0)
                    {
                        return apiResponse.Subjects;
                    }
                }


                // Fallback to old endpoint
                var oldGrades = await GetGradesAsync(studentId, token);
                if (oldGrades != null && oldGrades.Count > 0)
                {
                    var convertedSummaries = oldGrades.Select((g, index) => new StudentSubjectSummary
                    {
                        SubjectId = (index + 1).ToString(),
                        SubjectName = g.SubjectName,
                        SubjectCode = g.SubjectName?.Replace(" ", "").ToUpper() ?? "N/A",
                        TeacherName = "Professor",
                        WeightedAverage = (decimal)g.WeightedAverage,
                        TotalAbsences = g.TotalAbsences,
                        AllowedAbsences = g.AllowedAbsences,
                        FailedDueToAbsences = g.FailedDueToAbsences,
                        Status = g.FailedDueToAbsences ? "ExcludedByAbsences" : (g.WeightedAverage >= 10.0 ? "Approved" : "Failed"),
                        StatusDescription = g.FailedDueToAbsences ? "Excluded by Absences" : (g.WeightedAverage >= 10.0 ? "Approved" : "Failed")
                    }).ToList();

                    return convertedSummaries;
                }

                return new List<StudentSubjectSummary>();
            }
            catch (Exception)
            {
                return new List<StudentSubjectSummary>();
            }
        }

        public async Task<StudentSubjectDetail?> GetStudentSubjectDetailAsync(string studentId, string subjectId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{ApiBaseUrl}/grades/{studentId}/mobile/subject/{subjectId}";
                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<StudentSubjectGradeResponse>(json, JsonOptions);
                return apiResponse?.Subject;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SubjectInfo>> GetStudentSubjectsAsync(string studentId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"{ApiBaseUrl}/GradesAPI/{studentId}/subjects");

                if (!response.IsSuccessStatusCode)
                    return new List<SubjectInfo>();

                var json = await response.Content.ReadFromJsonAsync<SubjectListResponse>();

                return json?.Results ?? new List<SubjectInfo>();
            }
            catch (Exception)
            {
                return new List<SubjectInfo>();
            }
        }

        public async Task<SubjectGrade> GetSubjectGradeAsync(string studentId, string subjectCode, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"{ApiBaseUrl}/GradesAPI/{studentId}/subject/{subjectCode}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadFromJsonAsync<SubjectGradeResponse>();

                return json?.Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
