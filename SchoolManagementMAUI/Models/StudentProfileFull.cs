using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentProfileFull
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("email")] public string? Email { get; set; }
        [JsonPropertyName("fullName")] public string? FullName { get; set; }
        [JsonPropertyName("phoneNumber")] public string? PhoneNumber { get; set; }
        [JsonPropertyName("profilePictureUrl")] public string? ProfilePictureUrl { get; set; }
        [JsonPropertyName("profilePictureFullUrl")] public string? ProfilePictureFullUrl { get; set; }
        [JsonPropertyName("officialPhotoUrl")] public string? OfficialPhotoUrl { get; set; }
        [JsonPropertyName("officialPhotoFullUrl")] public string? OfficialPhotoFullUrl { get; set; }
        [JsonPropertyName("dateOfBirth")] public DateTimeOffset? DateOfBirth { get; set; }
        [JsonPropertyName("address")] public string? Address { get; set; }
    }
}
