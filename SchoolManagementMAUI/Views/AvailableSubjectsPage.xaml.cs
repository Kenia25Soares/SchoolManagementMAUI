using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class AvailableSubjectsPage : ContentPage
{
    private readonly AvailableSubjectsViewModel _viewModel;

    public AvailableSubjectsPage(AvailableSubjectsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAvailableSubjectsCommand.ExecuteAsync(null);
    }
}