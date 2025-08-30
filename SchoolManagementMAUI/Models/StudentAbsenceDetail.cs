using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class StudentAbsenceDetail
    {
        [JsonPropertyName("absenceId")] public int AbsenceId { get; set; }
        [JsonPropertyName("date")] public DateTime Date { get; set; }
        [JsonPropertyName("isJustified")] public bool IsJustified { get; set; }
        [JsonPropertyName("justification")] public string? Justification { get; set; }
    }
}
