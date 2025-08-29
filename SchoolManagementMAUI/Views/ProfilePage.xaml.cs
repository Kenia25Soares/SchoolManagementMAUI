namespace SchoolManagementMAUI.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(SchoolManagementMAUI.ViewModels.ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SchoolManagementMAUI.ViewModels.ProfileViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//dashboard");
    }
}