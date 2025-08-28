using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Models
{
    public class SubjectGrade
    {
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public double WeightedAverage { get; set; }
        public int TotalAbsences { get; set; }
        public int AllowedAbsences { get; set; }
        public bool FailedDueToAbsences { get; set; }
        public string Status { get; set; } // Aprovado/Reprovado/Excluído por faltas
        public List<GradeDetail> GradeDetails { get; set; } = new List<GradeDetail>();
        public List<AbsenceDetail> AbsenceDetails { get; set; } = new List<AbsenceDetail>();
    }
}
