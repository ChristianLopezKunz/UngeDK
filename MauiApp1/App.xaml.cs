using System.Globalization;

namespace MauiApp1
{
    public partial class App : Application
    {
        private AppTheme _currentTheme;

        public App()
        {
            InitializeComponent();

            // Set the culture to Danish (da-DK)
            var culture = new CultureInfo("da-DK");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Initialize the current theme
            _currentTheme = Application.Current.RequestedTheme;
            ApplyTheme();

            // Subscribe to theme changes
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        }

        private void ApplyTheme()
        {
            // Clear existing resources to avoid conflicts
            Resources.Clear();

            if (_currentTheme == AppTheme.Dark)
            {
                // Update resources for Dark Theme
                Resources["Primary"] = Resources.TryGetValue("PrimaryDark", out var primaryDark) ? primaryDark : Colors.Black;
                Resources["Secondary"] = Resources.TryGetValue("SecondaryDark", out var secondaryDark) ? secondaryDark : Colors.Gray;
                Resources["Background"] = Resources.TryGetValue("BackgroundDark", out var backgroundDark) ? backgroundDark : Colors.Black;
                Resources["Text"] = Resources.TryGetValue("TextDark", out var textDark) ? textDark : Colors.White;

                // Navigation Bar Resources for Dark Theme
                Resources["NavBarBackground"] = Resources.TryGetValue("NavBarBackgroundDark", out var navBackgroundDark) ? navBackgroundDark : Colors.Black;
                Resources["NavBarForeground"] = Resources.TryGetValue("NavBarForegroundDark", out var navForegroundDark) ? navForegroundDark : Colors.White;
            }
            else
            {
                // Update resources for Light Theme
                Resources["Primary"] = Resources.TryGetValue("PrimaryLight", out var primaryLight) ? primaryLight : Colors.Blue;
                Resources["Secondary"] = Resources.TryGetValue("SecondaryLight", out var secondaryLight) ? secondaryLight : Colors.LightGray;
                Resources["Background"] = Resources.TryGetValue("BackgroundLight", out var backgroundLight) ? backgroundLight : Colors.White;
                Resources["Text"] = Resources.TryGetValue("TextLight", out var textLight) ? textLight : Colors.Black;

                // Navigation Bar Resources for Light Theme
                Resources["NavBarBackground"] = Resources.TryGetValue("NavBarBackgroundLight", out var navBackgroundLight) ? navBackgroundLight : Colors.White;
                Resources["NavBarForeground"] = Resources.TryGetValue("NavBarForegroundLight", out var navForegroundLight) ? navForegroundLight : Colors.Black;
            }
        }


        public void ToggleTheme()
        {
            // Toggle the current theme
            _currentTheme = _currentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            ApplyTheme();
        }

        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            _currentTheme = e.RequestedTheme;
            ApplyTheme();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
