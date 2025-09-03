using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class EnrollmentRequestsPage : ContentPage
{
    public EnrollmentRequestsPage(EnrollmentRequestsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}