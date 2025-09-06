using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class GradesApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<Grade> SubjectGrades { get; set; } = new();
    }
}
