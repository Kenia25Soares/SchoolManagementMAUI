using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentGradeDetail
    {
        [JsonPropertyName("gradeId")] public int GradeId { get; set; }
        [JsonPropertyName("description")] public string? EvaluationType { get; set; } 
        [JsonPropertyName("weight")] public decimal Weight { get; set; }
        [JsonPropertyName("grade")] public decimal Value { get; set; } 
        [JsonPropertyName("date")] public DateTime Date { get; set; }
        [JsonPropertyName("observations")] public string? Observations { get; set; }
    }
}
