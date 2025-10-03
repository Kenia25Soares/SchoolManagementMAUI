using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SchoolManagementMAUI.Models
{    public class AlertInfo : ObservableObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;


        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;


        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;


        private bool _isRead;
        [JsonPropertyName("isRead")]
        public bool IsRead
        {
            get => _isRead;
            set => SetProperty(ref _isRead, value);
        }


        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }


        private DateTime? _readAt;
        [JsonPropertyName("readAt")]
        public DateTime? ReadAt
        {
            get => _readAt;
            set => SetProperty(ref _readAt, value);
        }


        [JsonPropertyName("subjectName")]
        public string? SubjectName { get; set; }


        [JsonPropertyName("className")]
        public string? ClassName { get; set; }


        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }
    }
}
