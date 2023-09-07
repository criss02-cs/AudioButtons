using AudioButtons.Views;

namespace AudioButtons
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ButtonPage), typeof(ButtonPage));
        }
    }
}