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

        public ApiAuthService()
        {
            //_client = new HttpClient(new HttpClientHandler
            var handler = new HttpClientHandler
            {
                // Para ignora o SSL de localhost com certificado inválido
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }


        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/users";

                using var testClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                });

                testClient.Timeout = TimeSpan.FromSeconds(5);
                testClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await testClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    email = email,
                    password = password,
                    rememberMe = false
                };

                var url = $"{ApiBaseUrl}/account/mobile-login";
                var content = new StringContent(
                    JsonSerializer.Serialize(loginRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = JsonSerializer.Deserialize<LoginResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || !result.Success || result.User == null)
                {
                    return null;
                }

                result.User.Token = result.Token;
                return result.User;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RecoverPasswordAsync(string email)
        {
            try
            {
                var url = $"{ApiBaseUrl}/account/recover-password";
                var request = new { email = email };
                var response = await _client.PostAsJsonAsync(url, request);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var result = JsonSerializer.Deserialize<RecoverPasswordResponse>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return result?.Success ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
