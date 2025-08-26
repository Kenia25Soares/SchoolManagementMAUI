using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}