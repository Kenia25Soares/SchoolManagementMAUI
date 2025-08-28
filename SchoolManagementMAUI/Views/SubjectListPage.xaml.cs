using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class SubjectListPage : ContentPage
{
    private readonly SubjectListViewModel _viewModel;

    public SubjectListPage(SubjectListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSubjectsAsync();
    }
}