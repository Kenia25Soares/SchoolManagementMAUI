using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentClass
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("academicYear")] public string? AcademicYear { get; set; }
        [JsonPropertyName("shift")] public string? Shift { get; set; }
        [JsonPropertyName("courseId")] public int CourseId { get; set; }
        [JsonPropertyName("isClosed")] public bool IsClosed { get; set; }
    }
}
