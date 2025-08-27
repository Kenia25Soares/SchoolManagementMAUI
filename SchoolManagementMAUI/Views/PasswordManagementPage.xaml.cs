using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class PasswordManagementPage : ContentPage
{
    private readonly PasswordManagementViewModel _viewModel;

    public PasswordManagementPage(PasswordManagementViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _viewModel.OnNavigatedTo();
    }
}