using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class AlertsPage : ContentPage
{
    public AlertsPage(AlertsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AlertsViewModel viewModel)
        {
            await viewModel.LoadAlertsAsync();
        }
    }
}