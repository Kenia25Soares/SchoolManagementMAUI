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

        // Controlar a password que foi recuperada
        private static bool _passwordRecovered = false;

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

        // Método quando a página é carregada
        public void OnNavigatedTo()
        {
            if (_passwordRecovered)
            {
                ShowPasswordRecoverySuccess();
                _passwordRecovered = false; 
            }
        }

        // Exibi a  mensagem de sucesso após recuperação da password
        public void ShowPasswordRecoverySuccess()
        {
            Message = "Password recovered successfully! You can now login with your new password.";
            HasMessage = true;

            // Limpa a mensagem após 5 segundos
            _ = Task.Run(async () =>
            {
                await Task.Delay(5000);
                Message = string.Empty;
                HasMessage = false;
            });
        }

        // A password foi recuperada
        public static void SetPasswordRecovered()
        {
            _passwordRecovered = true;
        }

        // Limpa a mensagem de erro ao alterar o email
        partial void OnEmailChanged(string value)
        {
            ClearMessage();
        }

        // Limpa a mensagem de erro ao alterar a password
        partial void OnPasswordChanged(string value)
        {
            ClearMessage();
        }

        private void ClearMessage()
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
                    appShell.UpdateMenuItems(true);
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
            await Shell.Current.GoToAsync("password-management");
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Validação @ pelo menos um ponto após o @
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
