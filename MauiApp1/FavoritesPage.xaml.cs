using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            Favorites = App.FavoritesList ?? new ObservableCollection<Job>();
            BindingContext = this; // Set the binding context to the page itself

            // Subscribe to changes in the Favorites list
            Favorites.CollectionChanged += OnFavoritesCollectionChanged;
        }

        // Unsubscribe when the page is disposed
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Favorites.CollectionChanged -= OnFavoritesCollectionChanged;
        }

        // Refresh UI if the favorites list changes
        private void OnFavoritesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // If necessary, notify the UI to refresh the binding
            OnPropertyChanged(nameof(Favorites));
        }

        private async void OnFrameTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Job selectedJob)
            {
                // Navigate to job details
                await Navigation.PushAsync(new JobDetailsPage(selectedJob));
            }
        }

        private async void OnRemoveFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Job job)
            {
                bool confirm = await DisplayAlert(
                    "Bekræft Fjernelse",
                    $"Er du sikker på, at du vil fjerne \"{job.JobTitle}\" fra dine favoritter?",
                    "Ja", "Nej");

                if (confirm)
                {
                    Favorites.Remove(job);
                }
            }
        }


        private async void OnJobSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Job selectedJob)
            {
                await Navigation.PushAsync(new JobDetailsPage(selectedJob));
            }

            // Deselect the item
            ((CollectionView)sender).SelectedItem = null;
        }

    }
}
