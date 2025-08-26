using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class GradesResult
    {
        public string? StudentName { get; set; }
        public List<Grade> SubjectGrades { get; set; }
        public double TotalAverage { get; set; }
        public bool IsClassClosed { get; set; }
        public List<object> Absences { get; set; }
    }
}
