using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services
{
    public class ApiEnrollmentService : IEnrollmentService
    {
        private readonly HttpClient _client;
        private readonly string ApiBaseUrl = "http://keniasoaresapi.somee.com/api";

        private readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiEnrollmentService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(handler);
        }

        public async Task<List<AvailableSubjectSummary>> GetAvailableSubjectsAsync(string studentId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{ApiBaseUrl}/enrollment/available-subjects/{studentId}";
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<AvailableSubjectsResponse>(json, JsonOptions);
                    return apiResponse?.Results ?? new List<AvailableSubjectSummary>();
                }

                return new List<AvailableSubjectSummary>();
            }
            catch (Exception ex)
            {
                return new List<AvailableSubjectSummary>();
            }
        }

        public async Task<bool> CreateEnrollmentRequestAsync(CreateEnrollmentRequest request, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(request, JsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{ApiBaseUrl}/enrollment/request";
                var response = await _client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<EnrollmentRequestSummary>> GetMyRequestsAsync(string studentId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{ApiBaseUrl}/enrollment/my-requests/{studentId}";
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<EnrollmentRequestsResponse>(json, JsonOptions);
                    return apiResponse?.Results ?? new List<EnrollmentRequestSummary>();
                }

                return new List<EnrollmentRequestSummary>();
            }
            catch (Exception ex)
            {
                return new List<EnrollmentRequestSummary>();
            }
        }
    }
}
