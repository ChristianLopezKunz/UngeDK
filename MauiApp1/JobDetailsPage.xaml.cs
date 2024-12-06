using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class JobDetailsPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();

        public JobDetailsPage(Job selectedJob)
        {
            InitializeComponent();
            BindingContext = selectedJob; // Set initial data for quick display
            LoadJobDetails(selectedJob.Id); // Load detailed data asynchronously
        }

        // Load detailed job data from the API
        private async void LoadJobDetails(string jobId)
        {
            try
            {
                var jobDetailsResponse = await _apiService.GetJobDetailsAsync(jobId);

                if (jobDetailsResponse != null && jobDetailsResponse.Status == "ok")
                {
                    BindingContext = jobDetailsResponse.Data; // Update BindingContext with detailed data
                }
                else
                {
                    await DisplayAlert("Error", "Unable to load job details.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading job details: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while fetching job details.", "OK");
            }
        }

        // Handle application URL button click
        private async void OnOpenApplicationUrlClicked(object sender, EventArgs e)
        {
            if (BindingContext is Job job && !string.IsNullOrWhiteSpace(job.ApplicationUrl))
            {
                try
                {
                    await Browser.OpenAsync(job.ApplicationUrl, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening URL: {ex.Message}");
                    await DisplayAlert("Error", "Unable to open the application link.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Info", "No application link available for this job.", "OK");
            }
        }
    }
}
