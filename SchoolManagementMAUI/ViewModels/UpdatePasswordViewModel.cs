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
    public partial class UpdatePasswordViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string currentPassword;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool isUpdatingPassword;

        [ObservableProperty]
        private bool canUpdatePassword = true;

        [ObservableProperty]
        private bool hasMessage = false;

        [ObservableProperty]
        private Color messageColor = Colors.Green;

        public UpdatePasswordViewModel(IAuthService authService, IUserSession userSession)
        {
            _authService = authService;
            _userSession = userSession;
        }

        partial void OnCurrentPasswordChanged(string value)
        {
            ClearMessage();
        }

        partial void OnNewPasswordChanged(string value)
        {
            ClearMessage();
        }

        partial void OnConfirmPasswordChanged(string value)
        {
            ClearMessage();
        }

        partial void OnMessageChanged(string value)
        {
            HasMessage = !string.IsNullOrEmpty(value);
        }

        private void ClearMessage()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                Message = string.Empty;
                HasMessage = false;
            }
        }

        [RelayCommand]
        private async Task UpdatePasswordAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentPassword))
                {
                    ShowMessage("Please enter your current password.", Colors.Red);
                    return;
                }

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

                IsUpdatingPassword = true;
                CanUpdatePassword = false;

                var user = _userSession.CurrentUser;
                if (user == null)
                {
                    ShowMessage("User session not found. Please login again.", Colors.Red);
                    return;
                }

            var success = await _authService.ChangePasswordAsync(CurrentPassword, NewPassword, ConfirmPassword, user.Token);
                if (success)
                {
                    ShowMessage(" Password updated successfully! Your password has been changed.", Colors.Green);
                    CurrentPassword = string.Empty;
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;

                    await Task.Delay(4000);
                    ClearMessage();
                }
                else
                {
                    ShowMessage("Error updating password. Please  check your current password and try again.", Colors.Red);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", Colors.Red);
            }
            finally
            {
                IsUpdatingPassword = false;
                CanUpdatePassword = true;
            }
        }

        [RelayCommand]
        private  async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("//dashboard");
        }

        private void ShowMessage(string text, Color color)
        {
            Message = text;
            MessageColor = color;
            HasMessage = true;
        }
    }
}
