using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class ThemePage : ContentPage
    {
        public ThemePage()
        {
            InitializeComponent();

            // Set the Switch state based on the current theme
            ThemeSwitch.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            // Toggle between Dark and Light themes
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
