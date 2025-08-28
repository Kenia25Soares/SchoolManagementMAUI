using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services
{
    public class ApiGradesService : IGradesService
    {
        private readonly HttpClient _client;
        private const string ApiBaseUrl = "https://10.0.2.2:7176/api"; // emulador Android
        //private const string ApiBaseUrl = "https://10.0.2.2:7021/api"; 

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

                var response = await _client.GetAsync($"{ApiBaseUrl}/GradesAPI/{studentId}");

                if (!response.IsSuccessStatusCode)
                    return new List<Grade>();

                var json = await response.Content.ReadFromJsonAsync<GradesApiResponse>();

                return json?.Results?.SubjectGrades ?? new List<Grade>();
            }
            catch (Exception)
            {
                return new List<Grade>();
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
