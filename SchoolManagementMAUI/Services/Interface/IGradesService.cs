using SchoolManagementMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IGradesService
    {
        Task<List<Grade>> GetGradesAsync(string studentId, string token);

        Task<List<SubjectInfo>> GetStudentSubjectsAsync(string studentId, string token);

        Task<SubjectGrade> GetSubjectGradeAsync(string studentId, string subjectCode, string token);

        Task<List<StudentSubjectSummary>> GetStudentSubjectsSummaryAsync(string studentId, string token);

        Task<StudentSubjectDetail?> GetStudentSubjectDetailAsync(string studentId, string subjectId, string token);
    }
}
