namespace SchoolManagementMAUI.Views;

public partial class PublicHomePage : ContentPage
{
    public PublicHomePage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }

    private async void OnViewCoursesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//public-courses");
    }

    private async void OnViewClassesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//public-classes");
    }
}