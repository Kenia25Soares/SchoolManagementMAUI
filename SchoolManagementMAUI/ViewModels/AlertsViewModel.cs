using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManagementMAUI.Models;
using SchoolManagementMAUI.Services.Interface;
using System.Collections.ObjectModel;

namespace SchoolManagementMAUI.ViewModels
{
    public partial class AlertsViewModel : ObservableObject
    {
        private readonly IAlertsService _alertsService;
        private readonly IUserSession _userSession;

        [ObservableProperty] private ObservableCollection<AlertInfo> alerts = new();
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool hasAlerts;
        [ObservableProperty] private bool hasUnreadAlerts;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool hasError;

        public AlertsViewModel(IAlertsService alertsService, IUserSession userSession)
        {
            _alertsService = alertsService;
            _userSession = userSession;
        }

        [RelayCommand]
        public async Task LoadAlertsAsync()
        {
            if (_userSession.CurrentUser == null || string.IsNullOrEmpty(_userSession.CurrentUser.Token))
            {
                ErrorMessage = "User session not available";
                HasError = true;
                return;
            }

            try
            {
                IsBusy = true;
                HasError = false;
                ErrorMessage = string.Empty;

                var response = await _alertsService.GetAllAlertsAsync(
                    _userSession.CurrentUser.Id,
                    _userSession.CurrentUser.Token);

                if (response?.Success == true)
                {
                    Alerts.Clear();
                    foreach (var alert in response.Alerts.OrderByDescending(a => a.CreatedAt))
                    {
                        Alerts.Add(alert);
                    }
                    HasAlerts = Alerts.Any();
                    HasUnreadAlerts = Alerts.Any(a => !a.IsRead);
                }
                else
                {
                    ErrorMessage = "Failed to load alerts";
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading alerts: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task MarkAlertAsReadAsync(AlertInfo alert)
        {
            if (_userSession.CurrentUser == null || string.IsNullOrEmpty(_userSession.CurrentUser.Token))
                return;

            if (alert.IsRead) return;

            try
            {
                var success = await _alertsService.MarkAlertAsReadAsync(alert.Id, _userSession.CurrentUser.Token);
                if (success)
                {
                    alert.IsRead = true;
                    alert.ReadAt = DateTime.UtcNow;
                    OnPropertyChanged(nameof(Alerts));
                    HasUnreadAlerts = Alerts.Any(a => !a.IsRead);
                    await LoadAlertsAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to mark alert as read: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task MarkAllAsReadAsync()
        {
            if (_userSession.CurrentUser == null || string.IsNullOrEmpty(_userSession.CurrentUser.Token))
                return;

            var unreadAlerts = Alerts.Where(a => !a.IsRead).ToList();
            if (!unreadAlerts.Any()) return;

            try
            {
                var alertIds = unreadAlerts.Select(a => a.Id).ToList();
                var success = await _alertsService.MarkMultipleAlertsAsReadAsync(alertIds, _userSession.CurrentUser.Token);

                if (success)
                {
                    foreach (var alert in unreadAlerts)
                    {
                        alert.IsRead = true;
                        alert.ReadAt = DateTime.UtcNow;
                    }
                    OnPropertyChanged(nameof(Alerts));
                    HasUnreadAlerts = false;
                    await Shell.Current.DisplayAlert("Success", "All alerts marked as read", "OK");
                    await LoadAlertsAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to mark alerts as read: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            await LoadAlertsAsync();
        }
    }
}
