using SchoolManagementMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IUserSession
    {
        User? CurrentUser { get; set; }
        bool IsLoggedIn { get; }
    }
}
