using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class UpdatePasswordPage : ContentPage
{
    public UpdatePasswordPage(UpdatePasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}