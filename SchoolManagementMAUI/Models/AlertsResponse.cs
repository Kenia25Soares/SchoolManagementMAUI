using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class AlertsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }


        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;


        [JsonPropertyName("alerts")]
        public List<AlertInfo> Alerts { get; set; } = new();


        [JsonPropertyName("unreadCount")]
        public int UnreadCount { get; set; }
    }
}
