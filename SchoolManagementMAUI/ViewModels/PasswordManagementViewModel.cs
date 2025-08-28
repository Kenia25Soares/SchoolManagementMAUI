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
    public partial class PasswordManagementViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        // Step Management
        [ObservableProperty]
        private bool isStep1 = true;

        [ObservableProperty]
        private bool isStep2 = false;

        [ObservableProperty]
        private bool isStep3 = false;

        // Email Input
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private bool canSendCode = true;

        [ObservableProperty]
        private bool isSendingCode = false;

        // Code Verification
        [ObservableProperty]
        private string verificationCode;

        [ObservableProperty]
        private bool canVerifyCode = true;

        [ObservableProperty]
        private bool isVerifyingCode = false;

        // New Password
        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool canResetPassword = true;

        [ObservableProperty]
        private bool isResettingPassword = false;

        // Common Properties
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool hasMessage = false;

        [ObservableProperty]
        private Color messageColor = Colors.Green;

        public PasswordManagementViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        // Método  resetar quando a página é carregada
        public void OnNavigatedTo()
        {
            ResetToStep1();
        }

        // Send Code
        partial void OnEmailChanged(string value)
        {
            ClearMessage();
        }

        [RelayCommand]
        private async Task SendCodeAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    ShowMessage("Please enter your email.", Colors.Red);
                    return;
                }

                if (!IsValidEmail(Email))
                {
                    ShowMessage("Please enter a valid email.", Colors.Red);
                    return;
                }

                IsSendingCode = true;
                CanSendCode = false;

                var success = await _authService.SendVerificationCodeAsync(Email);
                if (success)
                {
                    ShowMessage("Code sent successfully. Check your email.", Colors.Green);
                    await Task.Delay(1000); 
                    GoToStep2();
                }
                else
                {
                    ShowMessage("Error sending code. Please check if the email is correct.", Colors.Red);
                }
            }
            catch (Exception)
            {
                ShowMessage("Error processing code sending. Please try again.", Colors.Red);
            }
            finally
            {
                IsSendingCode = false;
                CanSendCode = true;
            }
        }

        // Verify Code
        partial void OnVerificationCodeChanged(string value)
        {
            ClearMessage();
        }

        [RelayCommand]
        private async Task VerifyCodeAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(VerificationCode))
                {
                    ShowMessage("Please enter the verification code.", Colors.Red);
                    return;
                }

                if (VerificationCode.Length < 4)
                {
                    ShowMessage("The code must have at least 4 digits.", Colors.Red);
                    return;
                }

                IsVerifyingCode = true;
                CanVerifyCode = false;

                var success = await _authService.VerifyCodeAsync(Email, VerificationCode);
                if (success)
                {
                    ShowMessage("Code verified successfully.", Colors.Green);
                    await Task.Delay(1000); 
                    GoToStep3();
                }
                else
                {
                    ShowMessage("Invalid code. Please check and try again.", Colors.Red);
                }
            }
            catch (Exception)
            {
                ShowMessage("Error verifying code. Please try again.", Colors.Red);
            }
            finally
            {
                IsVerifyingCode = false;
                CanVerifyCode = true;
            }
        }

        [RelayCommand]
        private async Task ResendCodeAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    ShowMessage("Email not found.", Colors.Red);
                    return;
                }

                var success = await _authService.SendVerificationCodeAsync(Email);
                if (success)
                {
                    ShowMessage("Code resent successfully.", Colors.Green);
                }
                else
                {
                    ShowMessage("Error resending code.", Colors.Red);
                }
            }
            catch (Exception)
            {
                ShowMessage("Error resending code. Please try again.", Colors.Red);
            }
        }

        // Reset Password
        partial void OnNewPasswordChanged(string value)
        {
            ClearMessage();
        }

        partial void OnConfirmPasswordChanged(string value)
        {
            ClearMessage();
        }

        [RelayCommand]
        private async Task ResetPasswordAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    ShowMessage("Please enter the new password.", Colors.Red);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    ShowMessage("Please confirm the new password.", Colors.Red);
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    ShowMessage("Passwords do not match.", Colors.Red);
                    return;
                }

                if (NewPassword.Length < 6)
                {
                    ShowMessage("The new password must have at least 6 characters.", Colors.Red);
                    return;
                }

                IsResettingPassword = true;
                CanResetPassword = false;

                var success = await _authService.ResetPasswordWithCodeAsync(Email, VerificationCode, NewPassword);

                if (success)
                {
                    ShowMessage(" Password changed successfully! You can now login with your new password.", Colors.Green);
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;

                    // A pass foi recuperada
                    LoginViewModel.SetPasswordRecovered();

                    // Navega para o login dps 3 segundos 
                    await Task.Delay(3000);

                    // Navega para o login
                    await Shell.Current.GoToAsync("///login");
                }
                else
                {
                    ShowMessage("Error changing password. Please try again.", Colors.Red);
                }
            }
            catch (Exception)
            {
                ShowMessage("Error processing password change. Please try again.", Colors.Red);
            }
            finally
            {
                IsResettingPassword = false;
                CanResetPassword = true;
            }
        }

        // Navigation
        [RelayCommand]
        private static async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("///login");
        }

        // Helper Methods
        private void GoToStep2()
        {
            IsStep1 = false;
            IsStep2 = true;
            IsStep3 = false;
        }

        private void GoToStep3()
        {
            IsStep1 = false;
            IsStep2 = false;
            IsStep3 = true;
        }

        private void ResetToStep1()
        {
            // Reset primeiro passo
            IsStep1 = true;
            IsStep2 = false;
            IsStep3 = false;

            // Limpa todos os campos
            Email = string.Empty;
            VerificationCode = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;

            // Limpa as mensagens
            ClearMessage();

            // Resetar o estados dos btn
            CanSendCode = true;
            CanVerifyCode = true;
            CanResetPassword = true;
            IsSendingCode = false;
            IsVerifyingCode = false;
            IsResettingPassword = false;
        }

        private void ShowMessage(string text, Color color)
        {
            Message = text;
            MessageColor = color;
            HasMessage = true;
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

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

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