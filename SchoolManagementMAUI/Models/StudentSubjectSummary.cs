using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentSubjectSummary
    {
        [JsonPropertyName("subjectId")] public string? SubjectId { get; set; } 
        [JsonPropertyName("subjectName")] public string? SubjectName { get; set; }
        [JsonPropertyName("subjectCode")] public string? SubjectCode { get; set; }
        [JsonPropertyName("teacherName")] public string? TeacherName { get; set; }
        [JsonPropertyName("weightedAverage")] public decimal? WeightedAverage { get; set; }
        [JsonPropertyName("totalAbsences")] public int TotalAbsences { get; set; }
        [JsonPropertyName("allowedAbsences")] public int AllowedAbsences { get; set; }
        [JsonPropertyName("failedDueToAbsences")] public bool FailedDueToAbsences { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; }
        [JsonPropertyName("statusDescription")] public string? StatusDescription { get; set; }
    }
}
