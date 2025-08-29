namespace SchoolManagementMAUI.Views;

public partial class CourseDetailPage : ContentPage
{
    private readonly SchoolManagementMAUI.ViewModels.CourseDetailViewModel _vm;
    public CourseDetailPage(SchoolManagementMAUI.ViewModels.CourseDetailViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    public void Initialize(int courseId, string? courseName = null)
    {
        _vm.Initialize(courseId, courseName);
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

    private void OnSubjectsTab(object sender, EventArgs e)
    {
        SubjectsView.IsVisible = true;
        ClassesView.IsVisible = false;
    }

    private void OnClassesTab(object sender, EventArgs e)
    {
        SubjectsView.IsVisible = false;
        ClassesView.IsVisible = true;
    }
}