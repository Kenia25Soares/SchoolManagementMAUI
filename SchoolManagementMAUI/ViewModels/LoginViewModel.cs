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
        private bool isRecoveringPassword;

        [ObservableProperty]
        private bool canRecoverPassword = true;

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
                    Message = "Por favor, preencha email e password.";
                    return;
                }

                var isConnected = await _authService.TestConnectionAsync();
                if (!isConnected)
                {
                    Message = "Erro de conectividade com o servidor.";
                    return;
                }

                var user = await _authService.LoginAsync(Email, Password);
                if (user == null)
                {
                    Message = "Credenciais inválidas.";
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
                Message = "Erro ao fazer login. Tente novamente.";
            }
        }

        [RelayCommand]
        private async Task RecoverPasswordAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    Message = "Por favor, insira seu email para recuperar a password.";
                    return;
                }

                IsRecoveringPassword = true;
                CanRecoverPassword = false;
                Message = "Enviando solicitação de recuperação...";

                var success = await _authService.RecoverPasswordAsync(Email);

                if (success)
                {
                    Message = "Email de recuperação enviado com sucesso! Verifique sua caixa de entrada.";
                }
                else
                {
                    Message = "Erro ao enviar email de recuperação. Verifique se o email está correto.";
                }
            }
            catch (Exception)
            {
                Message = "Erro ao processar recuperação de password. Tente novamente.";
            }
            finally
            {
                IsRecoveringPassword = false;
                CanRecoverPassword = true;
            }
        }
    }
}