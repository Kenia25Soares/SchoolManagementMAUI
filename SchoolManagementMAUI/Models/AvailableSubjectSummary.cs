using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class AvailableSubjectSummary  // Inscrição
    {
        [JsonPropertyName("subjectId")] 
        public int SubjectId { get; set; }


        [JsonPropertyName("subjectName")] 
        public string? SubjectName { get; set; }


        [JsonPropertyName("subjectCode")]
        public string? SubjectCode { get; set; }


        [JsonPropertyName("workload")]
        public int Workload { get; set; }


        [JsonPropertyName("allowedAbsences")] 
        public int AllowedAbsences { get; set; }


        [JsonPropertyName("hasPendingRequest")]
        public bool HasPendingRequest { get; set; }
    }
}
