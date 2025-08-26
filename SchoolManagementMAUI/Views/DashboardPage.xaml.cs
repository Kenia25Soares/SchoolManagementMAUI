using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DashboardViewModel viewModel)
        {
            viewModel.RefreshUserInfo();
        }
    }
}