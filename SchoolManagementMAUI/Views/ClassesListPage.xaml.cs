namespace SchoolManagementMAUI.Views;

public partial class ClassesListPage : ContentPage
{
    private readonly SchoolManagementMAUI.ViewModels.ClassesListViewModel _vm;
    private readonly SchoolManagementMAUI.Services.Interface.IUserSession _userSession;

    public ClassesListPage(SchoolManagementMAUI.ViewModels.ClassesListViewModel vm, SchoolManagementMAUI.Services.Interface.IUserSession userSession)
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
}