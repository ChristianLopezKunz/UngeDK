namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void OnToggleThemeButtonClicked(object sender, EventArgs e)
        {
            // Toggle the theme by accessing the App instance
            if (Application.Current is App app)
            {
                app.ToggleTheme();
            }
        }
    }
}