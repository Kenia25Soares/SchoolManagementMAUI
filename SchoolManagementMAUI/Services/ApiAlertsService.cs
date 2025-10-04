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
        private const string ApiBaseUrl = "http://keniasoaresapi.somee.com/api";
        
        // Try different URLs - API online primeiro
        private static readonly string[] PossibleApiUrls = {
            "http://keniasoaresapi.somee.com/api",         // API online 
            "https://keniasoaresapi.somee.com/api"         // API online HTTPS
        };
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiAlertsService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
                UseProxy = false, 
                UseCookies = false 
            };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "SchoolManagementMAUI/1.0");
            _client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _client.Timeout = TimeSpan.FromSeconds(60); 
        }

        public async Task<AlertsResponse?> GetAllAlertsAsync(string studentId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Try each possible URL until one works
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/alerts/{studentId}/all";
                    var response = await _client.GetAsync(url);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<AlertsResponse>(json, JsonOptions);
                    }
                    else
                    {
                        continue; 
                    }
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            return null;
        }

        public async Task<AlertsResponse?> GetUnreadAlertsAsync(string studentId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/alerts/{studentId}/unread";
                    var response = await _client.GetAsync(url);
                    if (!response.IsSuccessStatusCode) continue;
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<AlertsResponse>(json, JsonOptions);
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            return null;
        }

        public async Task<AlertsResponse?> GetRecentAlertsAsync(string studentId, int count, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/alerts/{studentId}/recent?count={count}";
                    var response = await _client.GetAsync(url);
                    if (!response.IsSuccessStatusCode) continue;
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<AlertsResponse>(json, JsonOptions);
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            return null;
        }

        public async Task<UnreadCountResponse?> GetUnreadCountAsync(string studentId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/alerts/{studentId}/unread-count";
                    var response = await _client.GetAsync(url);
                    if (!response.IsSuccessStatusCode) continue;
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UnreadCountResponse>(json, JsonOptions);
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            return null;
        }

        public async Task<bool> MarkAlertAsReadAsync(int alertId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/alerts/{alertId}/mark-read";
                    var response = await _client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode) return true;
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            return false;
        }

        public async Task<bool> MarkMultipleAlertsAsReadAsync(List<int> alertIds, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            foreach (var baseUrl in PossibleApiUrls)
            {
                try
                {
                    var url = $"{baseUrl}/alerts/mark-multiple-read";
                    var request = new MarkMultipleReadRequest { AlertIds = alertIds };
                    var response = await _client.PostAsJsonAsync(url, request, JsonOptions);
                    if (response.IsSuccessStatusCode) return true;
                }
                catch (Exception)
                {
                    continue; 
                }
            }
            return false;
        }
    }
}