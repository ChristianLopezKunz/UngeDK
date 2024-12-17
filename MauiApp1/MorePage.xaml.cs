using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MorePage : ContentPage
    {
        public MorePage()
        {
            InitializeComponent();
        }

        private void OnToggleThemeButtonClicked(object sender, EventArgs e)
        {
            // Toggle between Dark and Light themes
            var currentTheme = Application.Current.UserAppTheme;

            Application.Current.UserAppTheme =
                currentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;

            Console.WriteLine($"Theme switched to: {Application.Current.UserAppTheme}");
        }
    }
}
