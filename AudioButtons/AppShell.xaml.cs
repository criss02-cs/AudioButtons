using AudioButtons.Views;

namespace AudioButtons
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
            Routing.RegisterRoute(nameof(ButtonPage), typeof(ButtonPage));
        }
    }
}