using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class UpdateFullProfileResult
    {
        public bool Success { get; set; }
        public User? User { get; set; }
        public StudentProfileFull? Student { get; set; }
        public string? Raw { get; set; }
    }
}
