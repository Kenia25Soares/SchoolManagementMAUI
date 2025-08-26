using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class PasswordManagementPage : ContentPage
{
    public PasswordManagementPage(PasswordManagementViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}