using System;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel; // ViewModel instance
        private readonly ApiService _apiService = new ApiService(); // API Service

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel(); // Initialize ViewModel
            BindingContext = viewModel; // Bind ViewModel to the UI
        }

        // Håndter ændringer i søgefeltet dynamisk
        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = e.NewTextValue?.Trim(); // Søgeord fra feltet

            if (string.IsNullOrEmpty(keyword))
            {
                // Hvis søgefeltet er tomt, ryd FilteredItems
                viewModel.FilteredItems.Clear();
                return;
            }

            // Dynamisk søgning via API
            int maxResults = 50;
            int page = 1;

            var jobList = await _apiService.GetJobsAsync(keyword, maxResults, page);

            if (jobList != null && jobList.Status == "ok")
            {
                // Opdater FilteredItems i ViewModel med nye resultater
                viewModel.FilteredItems.Clear();
                foreach (var job in jobList.Jobs)
                {
                    viewModel.FilteredItems.Add($"{job.JobTitle} at {job.CompanyName}");
                }
            }
            else
            {
                // Hvis ingen jobs findes eller fejl opstår
                viewModel.FilteredItems.Clear();
                viewModel.FilteredItems.Add("Ingen resultater fundet.");
            }
        }
    }
}
