using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Services.Interface;
using SchoolManagementMAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class EnrollmentRequestsViewModel : ObservableObject
    {
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string message = string.Empty;

        [ObservableProperty]
        private bool isBusy = false;

        public EnrollmentRequestsViewModel(IUserSession userSession)
        {
            _userSession = userSession;
        }

        [RelayCommand]
        private async Task OpenAvailableSubjectsAsync()
        {
            var services = IPlatformApplication.Current?.Services;
            var page = services?.GetService<AvailableSubjectsPage>();
            if (page != null)
            {
                await Shell.Current.Navigation.PushAsync(page);
            }
        }

        [RelayCommand]
        private async Task OpenMyRequestsAsync()
        {
            var services = IPlatformApplication.Current?.Services;
            var page = services?.GetService<MyEnrollmentRequestsPage>();
            if (page != null)
            {
                await Shell.Current.Navigation.PushAsync(page);
            }
        }
    }
}
