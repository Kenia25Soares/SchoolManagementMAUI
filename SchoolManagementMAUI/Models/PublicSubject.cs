using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class PublicSubject
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("workload")] public int Workload { get; set; }
        [JsonPropertyName("allowedAbsences")] public int AllowedAbsences { get; set; }
    }
}
