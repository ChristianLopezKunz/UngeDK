using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class JobDetailsPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();

        public JobDetailsPage(Job selectedJob)
        {
            InitializeComponent();
            LoadJobDetails(selectedJob.Id);
        }
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
                }
            }
        }


        private async void LoadJobDetails(string jobId)
        {
            var jobDetailsResponse = await _apiService.GetJobDetailsAsync(jobId);

            if (jobDetailsResponse != null && jobDetailsResponse.Status == "ok")
            {
                BindingContext = jobDetailsResponse.Data; // Bind the detailed job object
            }
            else
            {
                await DisplayAlert("Error", "Unable to load job details.", "OK");
            }
        }

    }

}
