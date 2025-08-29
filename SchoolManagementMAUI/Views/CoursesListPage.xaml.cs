namespace SchoolManagementMAUI.Views;

public partial class CoursesListPage : ContentPage
{
    private readonly SchoolManagementMAUI.ViewModels.CoursesListViewModel _vm;
    private readonly SchoolManagementMAUI.Services.Interface.IUserSession _userSession;

    public CoursesListPage(SchoolManagementMAUI.ViewModels.CoursesListViewModel vm, SchoolManagementMAUI.Services.Interface.IUserSession userSession)
    {
        InitializeComponent();
        _vm = vm;
        _userSession = userSession;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (_userSession.IsLoggedIn)
        {
            await Shell.Current.GoToAsync("//dashboard");
        }
        else
        {
            await Shell.Current.GoToAsync("//home");
        }
    }

    private async void OnCourseTapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is SchoolManagementMAUI.Models.Course c)
        {
            var services = IPlatformApplication.Current?.Services;
            var detail = services?.GetService<SchoolManagementMAUI.Views.CourseDetailPage>();
            if (detail != null)
            {
                detail.Initialize(c.Id, c.Name);
                await Shell.Current.Navigation.PushAsync(detail);
            }
        }
    }
}
