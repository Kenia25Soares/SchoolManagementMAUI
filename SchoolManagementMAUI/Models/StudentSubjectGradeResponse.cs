using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentSubjectGradeResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
        [JsonPropertyName("studentId")] public string? StudentId { get; set; }
        [JsonPropertyName("result")] public StudentSubjectDetail? Subject { get; set; } // API returns "result", not "subject"
    }
}
