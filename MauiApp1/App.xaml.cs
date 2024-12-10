using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json; // For JSON serialization/deserialization
using Microsoft.Maui.Storage; // For file storage access

namespace MauiApp1
{
    public partial class App : Application
    {
        private AppTheme _currentTheme;
        // Global list to store favorite jobs
        public static ObservableCollection<Job> FavoritesList { get; set; }

        public App()
        {
            InitializeComponent();

            // Set the culture to Danish (da-DK)
            var culture = new CultureInfo("da-DK");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Retrieve the saved theme preference, default to Light if not found
            var savedTheme = Preferences.Get("themePreference", "Light");
            _currentTheme = savedTheme == "Dark" ? AppTheme.Dark : AppTheme.Light;
            ApplyTheme();

            // Subscribe to theme changes
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;

            // Initialize FavoritesList and load cached data
            FavoritesList = new ObservableCollection<Job>();
            Task.Run(async () => await LoadFavoritesFromStorageAsync());

            // Save to storage whenever the list changes
            FavoritesList.CollectionChanged += async (sender, e) => await SaveFavoritesToStorageAsync();
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

                // Frame Resources for Dark Theme
                Resources["FrameBorderColor"] = Resources.TryGetValue("FrameBorderColorDark", out var frameBorderDark) ? frameBorderDark : Colors.White;
                Resources["FrameBackgroundColor"] = Resources.TryGetValue("FrameBackgroundColorDark", out var frameBackgroundDark) ? frameBackgroundDark : Colors.Gray;
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

                // Frame Resources for Light Theme
                Resources["FrameBorderColor"] = Resources.TryGetValue("FrameBorderColorLight", out var frameBorderLight) ? frameBorderLight : Colors.Black;
                Resources["FrameBackgroundColor"] = Resources.TryGetValue("FrameBackgroundColorLight", out var frameBackgroundLight) ? frameBackgroundLight : Colors.White;
            }

            // Save the current theme to Preferences
            Preferences.Set("themePreference", _currentTheme == AppTheme.Dark ? "Dark" : "Light");
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

        // Save FavoritesList to local storage
        public static async Task SaveFavoritesToStorageAsync()
        {
            try
            {
                string favoritesJson = JsonSerializer.Serialize(FavoritesList);
                string filePath = FileSystem.AppDataDirectory + "/favorites.json";
                await File.WriteAllTextAsync(filePath, favoritesJson);
            }
            catch (Exception ex)
            {
                // Handle potential errors, e.g., log them
                Console.WriteLine($"Error saving favorites: {ex.Message}");
            }
        }

        // Load FavoritesList from local storage
        public static async Task LoadFavoritesFromStorageAsync()
        {
            try
            {
                string filePath = FileSystem.AppDataDirectory + "/favorites.json";

                if (File.Exists(filePath))
                {
                    string favoritesJson = await File.ReadAllTextAsync(filePath);
                    var loadedFavorites = JsonSerializer.Deserialize<ObservableCollection<Job>>(favoritesJson);

                    if (loadedFavorites != null)
                    {
                        foreach (var job in loadedFavorites)
                        {
                            FavoritesList.Add(job);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle potential errors, e.g., log them
                Console.WriteLine($"Error loading favorites: {ex.Message}");
            }
        }

    }
}
