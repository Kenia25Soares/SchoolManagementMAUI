namespace SchoolManagementMAUI.Views;

public partial class ClassGradesPage : ContentPage
{
    private readonly ViewModels.ClassGradesViewModel _vm;

    public ClassGradesPage(ViewModels.ClassGradesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadClassSubjectsAsync();
    }

    public Command BackCommand => new Command(async () =>
    {
        await Shell.Current.Navigation.PopAsync();
    });
}