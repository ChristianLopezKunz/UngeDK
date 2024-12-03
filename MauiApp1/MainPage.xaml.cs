using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        private readonly ApiService _apiService;
        private CancellationTokenSource _debounceCts;

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _viewModel = new MainViewModel();
            BindingContext = _viewModel;

            // Indledningsvis hent jobs
            Task.Run(() => LoadJobsFromApi());
        }

        private async Task LoadJobsFromApi()
        {
            var jobList = await _apiService.GetJobsAsync(max: 100, page: 1);
            if (jobList != null && jobList.Status == "ok")
            {
                _viewModel.Items.Clear();
                foreach (var job in jobList.Jobs)
                {
                    _viewModel.Items.Add($"{job.JobTitle} at {job.CompanyName}");
                }
                _viewModel.FilterItems(""); // Initial visning af alle jobs
            }
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _debounceCts?.Cancel(); // Annuller tidligere debounce-task
            _debounceCts = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, _debounceCts.Token); // 300ms debounce
                string searchText = e.NewTextValue?.Trim();

                if (string.IsNullOrEmpty(searchText))
                {
                    // Hvis ingen søgetekst, vis alle jobs
                    _viewModel.FilterItems("");
                }
                else
                {
                    // Hvis søgetekst, filtrer lokalt
                    _viewModel.FilterItems(searchText);

                    // Hvis ingen lokale matches, lav nyt API-kald
                    if (!_viewModel.FilteredItems.Any())
                    {
                        var jobList = await _apiService.GetJobsAsync(searchText, max: 50, page: 1);
                        if (jobList != null && jobList.Status == "ok")
                        {
                            foreach (var job in jobList.Jobs)
                            {
                                _viewModel.Items.Add($"{job.JobTitle} at {job.CompanyName}");
                            }
                            _viewModel.FilterItems(searchText);
                        }
                        else
                        {
                            _viewModel.FilteredItems.Clear();
                            _viewModel.FilteredItems.Add("Ingen resultater fundet.");
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorer annullerede tasks fra debounce
            }
        }
    }
}
