using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SchoolManagementMAUI.Services
{
    public class ApiAlertsService : IAlertsService
    {
        private readonly HttpClient _client;
        private const string ApiBaseUrl = "https://10.0.2.2:7176/api";
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiAlertsService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<AlertsResponse?> GetAllAlertsAsync(string studentId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/alerts/{studentId}/all";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlertsResponse>(json, JsonOptions);
        }

        public async Task<AlertsResponse?> GetUnreadAlertsAsync(string studentId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/alerts/{studentId}/unread";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlertsResponse>(json, JsonOptions);
        }

        public async Task<AlertsResponse?> GetRecentAlertsAsync(string studentId, int count, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/alerts/{studentId}/recent?count={count}";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlertsResponse>(json, JsonOptions);
        }

        public async Task<UnreadCountResponse?> GetUnreadCountAsync(string studentId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/alerts/{studentId}/unread-count";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UnreadCountResponse>(json, JsonOptions);
        }

        public async Task<bool> MarkAlertAsReadAsync(int alertId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/alerts/{alertId}/mark-read";
            var response = await _client.PostAsync(url, null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> MarkMultipleAlertsAsReadAsync(List<int> alertIds, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/alerts/mark-multiple-read";
            var request = new MarkMultipleReadRequest { AlertIds = alertIds };
            var response = await _client.PostAsJsonAsync(url, request, JsonOptions);
            return response.IsSuccessStatusCode;
        }
    }
}
