using SchoolManagementMAUI.Services.Interface;
using SchoolManagementMAUI.Views;

namespace SchoolManagementMAUI
{
    public partial class AppShell : Shell
    {
        private readonly IUserSession _userSession;

        public AppShell(IUserSession userSession)
        {
            InitializeComponent();
            _userSession = userSession;

            // Registro das rotas 
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("update-password", typeof(UpdatePasswordPage));
            Routing.RegisterRoute("password-management", typeof(PasswordManagementPage));
            Routing.RegisterRoute("alerts", typeof(AlertsPage));
            Routing.RegisterRoute("subject-grade", typeof(EnhancedSubjectDetailPage));

            // Desabilita o menu lateral
            FlyoutBehavior = FlyoutBehavior.Disabled;
        }

        public void UpdateFlyoutBehavior()
        {
            try
            {
                if (_userSession?.CurrentUser == null)
                {
                    if (FlyoutBehavior != FlyoutBehavior.Disabled)
                    {
                        FlyoutBehavior = FlyoutBehavior.Disabled;
                    }
                }
                else
                {
                    if (FlyoutBehavior != FlyoutBehavior.Flyout)
                    {
                        FlyoutBehavior = FlyoutBehavior.Flyout;
                    }
                }
            }
            catch (Exception) { /* Ignored */ }
        }

        public void UpdateMenuItems(bool isLoggedIn)
        {
            var logoutItem = Items.FirstOrDefault(x => x.Route == "logout");
            if (logoutItem != null)
                logoutItem.IsVisible = isLoggedIn;
        }

        private static void AddLogoutMenuItem() { }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            try
            {
                // Navega para logout
                if (args.Target.Location.ToString().Contains("logout"))
                {
                    args.Cancel();
                    PerformLogout();
                    return;
                }

                // Navegando para login e o user está logado
                if (args.Target.Location.ToString().Contains("login") && _userSession?.CurrentUser != null)
                {
                    args.Cancel();
                    return;
                }
            }
            catch (Exception) { /* Ignored */ }
        }

        public async void PerformLogout()
        {
            _userSession.CurrentUser = null;
            UpdateMenuItems(false);
            await Shell.Current.GoToAsync("//login");
            FlyoutBehavior = FlyoutBehavior.Disabled;
        }
    }
}

