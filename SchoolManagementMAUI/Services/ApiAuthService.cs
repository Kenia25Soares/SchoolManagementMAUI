using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services
{
    public class ApiAuthService : IAuthService
    {
        private readonly HttpClient _client;
        private static readonly string[] PossibleApiUrls = {
            "http://keniasoaresapi.somee.com/api",         // API online 
            "https://keniasoaresapi.somee.com/api",        // API online HTTPS
            "https://localhost:7176/api",         // HTTPS local
            "http://localhost:5100/api",          // HTTP local
            "http://10.0.2.2:5100/api"           // HTTP emulador
        };

        private const string ApiBaseUrl = "http://keniasoaresapi.somee.com/api"; 

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiAuthService()
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

        public async Task<User?> LoginAsync(string email, string password)
        {
            var request = new { email, password, rememberMe = false };

            foreach (var baseUrl in PossibleApiUrls)
            {
               try
                {
                    var url = $"{baseUrl}/account/mobile-login";
                    var requestJson = JsonSerializer.Serialize(request);
                    var response = await _client.PostAsJsonAsync(url, request);
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(jsonResponse, JsonOptions);
                        if (loginResponse?.Success == true && loginResponse.User != null)
                        {
                            loginResponse.User.Token = loginResponse.Token;
                            return loginResponse.User;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword, string confirmPassword, string? token = null)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/update-password";
                var payload = new
                {
                    currentPassword,
                    newPassword,
                    confirmPassword
                };

                if (!string.IsNullOrWhiteSpace(token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _client.PostAsJsonAsync(url, payload, JsonOptions);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return false;

                var result = JsonSerializer.Deserialize<GenericSuccessResponse>(json, JsonOptions);
                return result?.Success == true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendVerificationCodeAsync(string email)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/send-verification-code";
                var payload = new { email };
                var resp = await _client.PostAsJsonAsync(url, payload, JsonOptions);
                var json = await resp.Content.ReadAsStringAsync();
                return resp.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/verify-code";
                var payload = new { email, code };
                var resp = await _client.PostAsJsonAsync(url, payload, JsonOptions);
                var json = await resp.Content.ReadAsStringAsync();
                return resp.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordWithCodeAsync(string email, string code, string newPassword)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/reset-password-with-code";
                var payload = new
                {
                    email,
                    code,
                    newPassword,
                    confirmPassword = newPassword
                };
                var resp = await _client.PostAsJsonAsync(url, payload, JsonOptions);
                var json = await resp.Content.ReadAsStringAsync();
                return resp.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private class GenericSuccessResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
        }
    }
}