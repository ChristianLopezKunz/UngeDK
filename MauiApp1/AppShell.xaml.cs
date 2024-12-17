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

        // Toggle Dark/Light Mode
        private void OnToggleThemeButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Toggle between Light and Dark themes
                var currentTheme = Application.Current.UserAppTheme;
                Application.Current.UserAppTheme =
                    currentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;

                Console.WriteLine($"Theme switched to: {Application.Current.UserAppTheme}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling theme: {ex.Message}");
            }
        }

        private async void OnViewFavoritesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavoritesPage());
            FlyoutIsPresented = false;
        }

        private async void OnNavigateToMainPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
            FlyoutIsPresented = false;
        }
    }
}
