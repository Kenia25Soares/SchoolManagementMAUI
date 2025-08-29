using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentProfileFullResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("student")] public StudentProfileFull? Student { get; set; }
    }
}
