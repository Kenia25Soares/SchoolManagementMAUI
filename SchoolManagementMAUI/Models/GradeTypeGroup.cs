using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class GradeTypeGroup
    {
        public string? TypeName { get; set; }
        public string? TypeDisplayName { get; set; }
        public ObservableCollection<StudentGradeDetail> Grades { get; set; } = new();
        public decimal Average { get; set; }
        public int Count { get; set; }
    }
}
