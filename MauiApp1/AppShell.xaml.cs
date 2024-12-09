using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Navigating += OnShellNavigating;
        }

        private void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if (FlyoutIsPresented && e.Source == ShellNavigationSource.ShellItemChanged)
            {
                e.Cancel();
            }
        }

        private void OnToggleThemeButtonClicked(object sender, EventArgs e)
        {
            var currentTheme = Application.Current.RequestedTheme;
            Application.Current.UserAppTheme = currentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;

            FlyoutIsPresented = false;
        }

        private async void OnViewFavoritesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavoritesPage());
        }
    }
}
