using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class MyEnrollmentRequestsPage : ContentPage
{
    private readonly MyEnrollmentRequestsViewModel _viewModel;

    public MyEnrollmentRequestsPage(MyEnrollmentRequestsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMyRequestsCommand.ExecuteAsync(null);
    }
}