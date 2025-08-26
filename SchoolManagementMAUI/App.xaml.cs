using SchoolManagementMAUI.Views;

namespace SchoolManagementMAUI
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();

            MainPage =  appShell;
        }
    }
}
