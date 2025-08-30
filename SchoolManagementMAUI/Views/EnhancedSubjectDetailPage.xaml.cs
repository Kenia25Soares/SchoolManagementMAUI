using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class EnhancedSubjectDetailPage : ContentPage
{
    private readonly EnhancedSubjectDetailViewModel _vm;

    public EnhancedSubjectDetailPage(EnhancedSubjectDetailViewModel vm)
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

    public Command BackCommand => new Command(async () =>
    {
        await Shell.Current.Navigation.PopAsync();
    });
}