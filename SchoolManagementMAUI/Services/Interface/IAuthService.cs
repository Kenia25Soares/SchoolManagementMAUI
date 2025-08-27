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

        // Method for logged-in user to change password
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword, string confirmPassword, string? token = null);

        // Methods for code-based password reset
        Task<bool> SendVerificationCodeAsync(string email);
        Task<bool> VerifyCodeAsync(string email, string code);
        Task<bool> ResetPasswordWithCodeAsync(string email, string code, string newPassword);
    }
}
