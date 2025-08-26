using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }


        [JsonPropertyName("message")]
        public string Message { get; set; }


        [JsonPropertyName("token")]
        public string Token { get; set; }


        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}
