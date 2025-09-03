using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class CourseWithSubjects
    {
        public Course Course { get; set; }
        public ObservableCollection<PublicSubject> Subjects { get; set; } = new();
        public bool IsExpanded { get; set; } = false;
    }
}
