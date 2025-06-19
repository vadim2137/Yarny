using Yarny.Pages;

namespace Yarny
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(EmailConfirmationPage), typeof(EmailConfirmationPage));
            Routing.RegisterRoute(nameof(PasswordRecoveryPage1), typeof(PasswordRecoveryPage1));


            Routing.RegisterRoute(nameof(CounterPage), typeof(CounterPage));
            Routing.RegisterRoute(nameof(PostFeedPage), typeof(PostFeedPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));

            // Добавьте другие страницы по мере необходимости
        }
    }
}