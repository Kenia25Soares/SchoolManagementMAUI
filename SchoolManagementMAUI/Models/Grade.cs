using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class Grade
    {
        public string SubjectName { get; set; }
        public double WeightedAverage { get; set; }
        public int TotalAbsences { get; set; }
        public int AllowedAbsences { get; set; }
        public bool FailedDueToAbsences { get; set; }
    }
}
