using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class CreateEnrollmentRequest  // Pedido de inscrição
    {
        [JsonPropertyName("studentId")] 
        public string? StudentId { get; set; }


        [JsonPropertyName("subjectId")] 
        public int SubjectId { get; set; }


        [JsonPropertyName("description")] 
        public string? Description { get; set; }
    }
}
