using SchoolManagementMAUI.Services.Interface;

namespace SchoolManagementMAUI
{
    public partial class AppShell : Shell
    {
        private readonly IUserSession _userSession;

        public AppShell(IUserSession userSession)
        {
            InitializeComponent();

            _userSession = userSession;

            FlyoutBehavior = FlyoutBehavior.Disabled;
        }

        public void UpdateFlyoutBehavior()
        {
            try
            {
                if (_userSession?.CurrentUser == null)
                {
                    FlyoutBehavior = FlyoutBehavior.Disabled;
                }
                else
                {
                    FlyoutBehavior = FlyoutBehavior.Flyout;
                    AddLogoutMenuItem();
                }
            }
            catch (Exception)
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
            }
        }

        private void AddLogoutMenuItem()
        {
            try
            {
                var existingLogout = Items.FirstOrDefault(item => item.Route == "logout");
                if (existingLogout != null) return;

                var logoutItem = new ShellContent
                {
                    Title = "Logout",
                    Route = "logout",
                    Content = new ContentPage
                    {
                        Content = new Label
                        {
                            Text = "Logout",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    }
                };

                Items.Add(logoutItem);
            }
            catch (Exception)
            {
                // Ignora os erros ao adicionar logout
            }
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            try
            {
                if (args.Target.Location.ToString().Contains("logout"))
                {
                    args.Cancel();
                    PerformLogout();
                }
            }
            catch (Exception)
            {
                // Ignora erros de navegação
            }
        }

        private async void PerformLogout()
        {
            try
            {
                _userSession.CurrentUser = null;
                FlyoutBehavior = FlyoutBehavior.Disabled;
                await Shell.Current.GoToAsync("//login");
            }
            catch (Exception)
            {
                // Ignora erros de logout
            }

        }
    }
}
