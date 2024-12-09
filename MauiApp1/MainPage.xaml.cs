using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        private readonly ApiService _apiService;
        private CancellationTokenSource _debounceCts;
        public ICommand NavigateToFavoritesCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _viewModel = new MainViewModel();
            BindingContext = _viewModel;

            // Fetch jobs initially
            Task.Run(() => LoadJobsFromApi());

            NavigateToFavoritesCommand = new Command(async () => await NavigateToFavorites());
        }

        private async Task LoadJobsFromApi()
        {
            var jobList = await _apiService.GetJobsAsync(max: 100, page: 1);
            if (jobList != null && jobList.Status == "ok")
            {
                _viewModel.Items.Clear();
                foreach (var job in jobList.Jobs)
                {
                    _viewModel.Items.Add(job); // Add the full Job object
                }
                _viewModel.FilterItems(""); // Show all jobs initially
            }
        }

        private void OnRegionFilterChanged(object sender, EventArgs e)
        {
            var selectedRegion = RegionPicker.SelectedItem?.ToString() ?? "Alle";
            var searchText = SearchBarControl.Text;
            _viewModel.FilterItems(searchText, selectedRegion);
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _debounceCts?.Cancel(); // Cancel previous debounce task
            _debounceCts = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, _debounceCts.Token); // 300ms debounce
                string searchText = e.NewTextValue?.Trim();
                var selectedRegion = RegionPicker.SelectedItem?.ToString() ?? "Alle";
                _viewModel.FilterItems(searchText, selectedRegion);

                if (string.IsNullOrEmpty(searchText))
                {
                    _viewModel.FilterItems(""); // Show all jobs
                    return;
                }

                // Perform local filtering first
                _viewModel.FilterItems(searchText);

                // If no local matches, fetch more from the API
                if (!_viewModel.FilteredItems.Any())
                {
                    var jobList = await _apiService.GetJobsAsync(searchText, max: 50, page: 1);
                    if (jobList != null && jobList.Status == "ok")
                    {
                        foreach (var job in jobList.Jobs)
                        {
                            if (!_viewModel.Items.Any(existing => existing.Id == job.Id)) // Check duplicates by Job ID
                            {
                                _viewModel.Items.Add(job);
                            }
                        }

                        // Reapply filtering with the updated items
                        _viewModel.FilterItems(searchText);
                    }
                    else
                    {
                        // If API returns no results
                        _viewModel.FilteredItems.Clear();
                        _viewModel.FilteredItems.Add(new Job
                        {
                            JobTitle = "Ingen resultater fundet.",
                            CompanyName = string.Empty
                        });
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore canceled tasks from debounce
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

        private async Task NavigateToFavorites()
        {
            await Navigation.PushAsync(new FavoritesPage());
        }

    }
}
