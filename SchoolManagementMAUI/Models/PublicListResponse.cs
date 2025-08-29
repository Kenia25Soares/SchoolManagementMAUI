using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class PublicListResponse<T>
    {
        [JsonPropertyName("items")] public List<T>? Items { get; set; }
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("success")] public bool Success { get; set; }
    }
}
