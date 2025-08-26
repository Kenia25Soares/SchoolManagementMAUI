using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool hasMessage = false;

        public LoginViewModel(IAuthService authService, IUserSession userSession)
        {
            _authService = authService;
            _userSession = userSession;
        }

        partial void OnEmailChanged(string value)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                Message = string.Empty;
                HasMessage = false;
            }
        }

        partial void OnPasswordChanged(string value)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                Message = string.Empty;
                HasMessage = false;
            }
        }

        partial void OnMessageChanged(string value)
        {
            HasMessage = !string.IsNullOrEmpty(value);
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    Message = "Please enter email and password.";
                    return;
                }

                var user = await _authService.LoginAsync(Email, Password);
                if (user == null)
                {
                    Message = "Invalid password.";
                    return;
                }

                // Verifica se o utilizador tem a role "Student"
                if (user.Roles == null || !user.Roles.Contains("Student"))
                {
                    Message = "Access denied.";
                    return;
                }

                _userSession.CurrentUser = user;

                if (Shell.Current is AppShell appShell)
                {
                    appShell.UpdateFlyoutBehavior();
                    appShell.UpdateMenuItems();
                }

                await Shell.Current.GoToAsync("//dashboard");
            }
            catch (Exception)
            {
                Message = "Error logging in. Please try again.";
            }
        }



        [RelayCommand]
        private static async Task GoToRecoverPasswordAsync()
        {
            await Shell.Current.GoToAsync("///password-management");
        }



        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Validação simples: deve ter @ e pelo menos um ponto após o @
            var atIndex = email.IndexOf('@');
            if (atIndex <= 0 || atIndex == email.Length - 1)
                return false;

            var domainPart = email[(atIndex + 1)..];
            if (!domainPart.Contains('.'))
                return false;

            return true;
        }
    }
}
