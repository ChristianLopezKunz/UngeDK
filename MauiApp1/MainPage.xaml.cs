using System;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;
        private readonly ApiService _apiService = new ApiService();

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            BindingContext = viewModel;
        }

        // Opdater filter på søgetekstændringer
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.FilterItems(e.NewTextValue);
        }

        // Når knappen trykkes for at hente jobdetaljer
        private async void OnFetchJobDetailsClicked(object sender, EventArgs e)
        {
            int jobId = 2678108;  // Test med et eksempel jobID

            // Hent jobdetaljer via ApiService
            var jobDetails = await _apiService.GetJobDetailAsync(jobId);

            if (jobDetails != null && jobDetails.Status == "ok")
            {
                // Hvis jobdetaljer findes, vis dem
                await DisplayAlert("Job Details",
                    $"Job Title: {jobDetails.Data.JobTitle}\n" +
                    $"Company: {jobDetails.Data.CompanyName}\n" +
                    $"Apply: {jobDetails.Data.ApplicationUrl}",
                    "OK");
            }
            else
            {
                // Hvis jobdetaljer ikke kan hentes, vis en fejl
                await DisplayAlert("Error", "Kunne ikke hente jobdetaljer.", "OK");
            }
        }
    }
}
