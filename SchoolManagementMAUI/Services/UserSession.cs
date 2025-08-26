using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services
{
    public class UserSession : IUserSession
    {
        public User? CurrentUser { get; set; }

        public bool IsLoggedIn => CurrentUser != null;
    }
}
