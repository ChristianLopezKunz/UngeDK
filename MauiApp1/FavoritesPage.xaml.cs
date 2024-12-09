using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp1
{
    public partial class FavoritesPage : ContentPage
    {
        public ObservableCollection<Job> Favorites { get; set; }

        public FavoritesPage()
        {
            InitializeComponent();

            // Bind FavoritesList from App.xaml.cs to the page
            Favorites = App.FavoritesList;
            BindingContext = this; // Set the binding context to the page itself
        }

        // Handle the "Remove" button click event
        private void OnRemoveFavoriteClicked(object sender, EventArgs e)
        {
            // Get the job to remove
            if (sender is Button button && button.CommandParameter is Job job)
            {
                // Remove the job from the global Favorites list
                if (App.FavoritesList.Contains(job))
                {
                    App.FavoritesList.Remove(job);
                }
            }
        }
    }
}