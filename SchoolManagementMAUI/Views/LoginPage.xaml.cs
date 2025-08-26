using SchoolManagementMAUI.Services;
using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}