using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services
{
    public class ApiAuthService : IAuthService
    {
        private readonly HttpClient _client;
        private const string ApiBaseUrl = "https://10.0.2.2:7176/api";

        // JsonSerializerOptions reutilizado para melhor performance
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiAuthService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }



        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/mobile-login";
                var request = new { email, password };
                var response = await _client.PostAsJsonAsync(url, request);

                if (!response.IsSuccessStatusCode)
                    return null;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(jsonResponse, JsonOptions);

                if (loginResponse?.Success == true && loginResponse.User != null)
                {
                    loginResponse.User.Token = loginResponse.Token;
                    return loginResponse.User;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword, string confirmPassword, string? token = null)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/update-password";
                var request = new { currentPassword, newPassword, confirmPassword };

                // Usar o cliente existente que já tem SSL validation desabilitado
                if (!string.IsNullOrEmpty(token))
                {
                    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _client.PostAsJsonAsync(url, request);

                return response.IsSuccessStatusCode;
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
                var request = new { email };
                var response = await _client.PostAsJsonAsync(url, request);

                return response.IsSuccessStatusCode;
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
                var request = new { email, code };
                var response = await _client.PostAsJsonAsync(url, request);

                return response.IsSuccessStatusCode;
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
                var request = new { email, code, newPassword };
                var response = await _client.PostAsJsonAsync(url, request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
