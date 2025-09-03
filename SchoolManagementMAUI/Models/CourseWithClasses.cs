using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class CourseWithClasses
    {
        public Course Course { get; set; }
        public ObservableCollection<StudentClass> Classes { get; set; } = new();
    }
}
