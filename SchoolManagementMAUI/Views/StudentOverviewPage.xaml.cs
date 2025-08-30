using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class StudentOverviewPage : ContentPage
{
    private readonly StudentOverviewViewModel _vm;

    public StudentOverviewPage(StudentOverviewViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadStudentInfoAsync();
    }
}