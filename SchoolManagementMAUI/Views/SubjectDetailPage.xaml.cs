using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class SubjectDetailPage : ContentPage
{
    private readonly SubjectDetailViewModel _vm;

    public SubjectDetailPage(SubjectDetailViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    public void Initialize(string? subjectId, string? subjectName = null)
    {
        _vm.Initialize(subjectId, subjectName);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadDetailAsync();
    }
}