using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

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

        // Handle the "Remove" button click event with confirmation
        private async void OnRemoveFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Job job)
            {
                // Display confirmation popup
                bool confirm = await DisplayAlert(
                    "Bekræft Fjernelse",
                    $"Er du sikker på, at du vil fjerne \"{job.JobTitle}\" fra dine favoritter?",
                    "Ja", "Nej");

                if (confirm)
                {
                    // Remove the job from the list if confirmed
                    if (Favorites.Contains(job))
                    {
                        Favorites.Remove(job);
                    }
                }
            }
        }

        // Handle job selection to navigate to the details page
        private async void OnJobSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Job selectedJob)
            {
                await Navigation.PushAsync(new JobDetailsPage(selectedJob));
            }

            // Deselect the item to avoid lingering selection
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
