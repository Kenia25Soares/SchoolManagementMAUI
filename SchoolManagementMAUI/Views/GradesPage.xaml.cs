using SchoolManagementMAUI.ViewModels;

namespace SchoolManagementMAUI.Views;

public partial class GradesPage : ContentPage
{
    public GradesPage(GradesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        // Carrega as notas 
        Loaded += async (s, e) => await vm.LoadGradesAsync();
    }
}