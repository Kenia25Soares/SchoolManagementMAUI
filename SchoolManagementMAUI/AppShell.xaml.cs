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

            // Registra as rotas programaticamente
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("update-password", typeof(UpdatePasswordPage));
            Routing.RegisterRoute("password-management", typeof(PasswordManagementPage));

            // Inicialmente desabilita o menu lateral
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

        public void UpdateMenuItems()
        {
            try
            {
                // Remove os itens dinâmicos do menu
                var itemsToRemove = new List<ShellItem>();

                // Coleta todos os itens que precisam ser removidos
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    var item = Items[i];
                    if (item.Route == "logout" ||
                        item.Route == "IMPL_logout" ||
                        item.Route == "project" ||
                        item.Route == "login" ||
                        item.Title == "School Management" ||
                        item.Title == "Login")
                    {
                        itemsToRemove.Add(item);
                    }
                }

                // Remove os itens que foi coletado
                foreach (var item in itemsToRemove)
                {
                    Items.Remove(item);
                }

                // Só adiciona logout se o utilizador estiver logado
                if (_userSession?.CurrentUser != null)
                {
                    // Verifica se já existe um item logout 
                    var existingLogout = Items.FirstOrDefault(item => item.Route == "logout" || item.Route == "IMPL_logout");
                    if (existingLogout == null)
                    {
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
                }
            }
            catch (Exception) { /* Ignored */ }
        }

        private static void AddLogoutMenuItem()
        {
            // método não é mais usado, mantido para compatibilidade
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            try
            {
                // Se está navegando para logout, executa o logout
                if (args.Target.Location.ToString().Contains("logout"))
                {
                    args.Cancel();
                    PerformLogout();
                    return;
                }

                // Se está navegando para login e o utilizador está logado
                if (args.Target.Location.ToString().Contains("login") && _userSession?.CurrentUser != null)
                {
                    args.Cancel();
                    return;
                }
            }
            catch (Exception) { /* Ignored */ }
        }

        private async void PerformLogout()
        {
            try
            {
                // Limpa a sessão do utilizador
                _userSession.CurrentUser = null;

                // Desabilita o menu lateral
                FlyoutBehavior = FlyoutBehavior.Disabled;

                // Limpa os itens dinâmicos do menu
                var itemsToRemove = new List<ShellItem>();

                // Ve todos os itens que precisam ser removidos
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    var item = Items[i];
                    if (item.Route == "logout" ||
                        item.Route == "IMPL_logout" ||
                        item.Route == "project" ||
                        item.Route == "login" ||
                        item.Title == "School Management" ||
                        item.Title == "Login")
                    {
                        itemsToRemove.Add(item);
                    }
                }

                // Remove os itens coletados
                foreach (var item in itemsToRemove)
                {
                    Items.Remove(item);
                }

                // Força a navegação para o login usando uma  rota  
                await Shell.Current.GoToAsync("login", false);

                // Desabilita o menu lateral
                FlyoutBehavior = FlyoutBehavior.Disabled;
            }
            catch (Exception)
            {
                
                try
                {
                    await Shell.Current.GoToAsync("login", false);
                }
                catch { /* Ignored */ }
            }
        }
    }
}
