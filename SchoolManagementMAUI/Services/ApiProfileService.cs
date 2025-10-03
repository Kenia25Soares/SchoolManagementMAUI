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
    public class ApiProfileService : IProfileService
    {
        private readonly HttpClient _client;
        private const string ApiBaseUrl = "http://keniasoaresapi.somee.com/api";
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiProfileService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<StudentProfileFull?> GetStudentProfileFullAsync(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{ApiBaseUrl}/account/student-profile-full";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<StudentProfileFullResponse>(json, JsonOptions);
            return dto?.Student;
        }

        public async Task<UpdateFullProfileResponse> UpdateFullProfileAsync(ProfileUpdateData updateData, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{ApiBaseUrl}/account/profile/full";

            using var form = new MultipartFormDataContent();
            
            // Campos obrigatório
            form.Add(new StringContent(updateData.FullName ?? string.Empty), "fullName");
            form.Add(new StringContent(updateData.Email ?? string.Empty), "email");
            
            // Campos opcionais
            if (!string.IsNullOrWhiteSpace(updateData.PhoneNumber)) 
                form.Add(new StringContent(updateData.PhoneNumber), "phoneNumber");
            
            if (updateData.DateOfBirth.HasValue) 
                form.Add(new StringContent(updateData.DateOfBirth.Value.ToString("yyyy-MM-dd")), "dateOfBirth");
            
            if (!string.IsNullOrWhiteSpace(updateData.Address)) 
                form.Add(new StringContent(updateData.Address), "address");

            // Image opcionais
            if (updateData.ProfilePictureStream != null)
            {
                var content = new StreamContent(updateData.ProfilePictureStream);
                content.Headers.ContentType = new MediaTypeHeaderValue(updateData.ProfilePictureContentType ?? "image/jpeg");
                form.Add(content, "profilePicture", updateData.ProfilePictureFileName ?? "profile.jpg");
            }

            if (updateData.OfficialPhotoStream != null)
            {
                var content = new StreamContent(updateData.OfficialPhotoStream);
                content.Headers.ContentType = new MediaTypeHeaderValue(updateData.OfficialPhotoContentType ?? "image/jpeg");
                form.Add(content, "officialPhoto", updateData.OfficialPhotoFileName ?? "official.jpg");
            }

            try
            {
                var response = await _client.PutAsync(url, form);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new UpdateFullProfileResponse { Success = false, Message = $"API error: {response.StatusCode}" };
                }

                var result = JsonSerializer.Deserialize<UpdateFullProfileResponse>(jsonResponse, JsonOptions) ?? new UpdateFullProfileResponse { Success = false };
                return result;
            }
            catch (Exception ex)
            {
                return new UpdateFullProfileResponse { Success = false, Message = ex.Message };
            }
        }
    }
}
