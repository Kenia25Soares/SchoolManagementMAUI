using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class MarkMultipleReadRequest
    {
        [JsonPropertyName("alertIds")]
        public List<int> AlertIds { get; set; } = new();
    }
}
