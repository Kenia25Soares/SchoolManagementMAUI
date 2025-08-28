using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class SubjectListResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public List<SubjectInfo> Results { get; set; }
    }
}
