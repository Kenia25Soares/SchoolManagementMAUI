using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SchoolManagementMAUI.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }


        [JsonPropertyName("fullName")]
        public string FullName { get; set; }


        [JsonPropertyName("email")]
        public string Email { get; set; }


        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }


        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }


        public string Token { get; set; }
    }
}
