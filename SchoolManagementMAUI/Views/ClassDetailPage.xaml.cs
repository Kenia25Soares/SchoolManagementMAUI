namespace SchoolManagementMAUI.Views;

public partial class ClassDetailPage : ContentPage
{
    private readonly SchoolManagementMAUI.ViewModels.ClassDetailViewModel _vm;
    public ClassDetailPage(SchoolManagementMAUI.ViewModels.ClassDetailViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    public void Initialize(int classId, int courseId, string? className = null, string? courseName = null)
    {
        _vm.Initialize(classId, courseId, className, courseName);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}