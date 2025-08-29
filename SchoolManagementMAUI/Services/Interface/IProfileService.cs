using SchoolManagementMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IProfileService
    {
        Task<StudentProfileFull?> GetStudentProfileFullAsync(string token);
        Task<UpdateFullProfileResult> UpdateFullProfileAsync(ProfileUpdateData updateData, string token);
    }
}
