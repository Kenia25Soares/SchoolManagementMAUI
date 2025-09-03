using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class EnrollmentRequestsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }


        [JsonPropertyName("message")] 
        public string? Message { get; set; }


        [JsonPropertyName("results")]
        public List<EnrollmentRequestSummary>? Results { get; set; }
    }
}
