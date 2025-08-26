using SchoolManagementMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.Services.Interface
{
    public interface IAuthService
    {

        Task<User?> LoginAsync(string email, string password);

        Task<bool> TestConnectionAsync();

        Task<bool> RecoverPasswordAsync(string email);
    }
}
