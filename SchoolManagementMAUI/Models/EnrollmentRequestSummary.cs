using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class EnrollmentRequestSummary
    {
        [JsonPropertyName("requestId")]
        public int RequestId { get; set; }


        [JsonPropertyName("subjectName")]
        public string? SubjectName { get; set; }


        [JsonPropertyName("subjectCode")] 
        public string? SubjectCode { get; set; }


        [JsonPropertyName("description")] 
        public string? Description { get; set; }


        [JsonPropertyName("status")]
        public string? Status { get; set; }


        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonPropertyName("responseMessage")]
        public string? ResponseMessage { get; set; }


        [JsonPropertyName("processedByName")] 
        public string? ProcessedByName { get; set; }


        [JsonPropertyName("processedDate")]
        public DateTime? ProcessedDate { get; set; }
    }
}
