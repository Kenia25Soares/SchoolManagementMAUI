using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class AlertInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;


        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;


        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;


        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }


        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }


        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; }


        [JsonPropertyName("subjectName")]
        public string? SubjectName { get; set; }


        [JsonPropertyName("className")]
        public string? ClassName { get; set; }


        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }
    }
}
