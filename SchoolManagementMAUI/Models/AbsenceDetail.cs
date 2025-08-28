using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class AbsenceDetail
    {
        public DateTime Date { get; set; }
        public string Justification { get; set; }
        public bool IsJustified { get; set; }
    }
}
