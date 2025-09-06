using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentSubjectsListResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
        [JsonPropertyName("studentId")] public string? StudentId { get; set; }
        [JsonPropertyName("results")] public List<StudentSubjectSummary>? Results { get; set; } // Returns "results", not "subjects"

        public List<StudentSubjectSummary>? Subjects => Results;
    }
}
