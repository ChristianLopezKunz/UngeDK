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

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _viewModel = new MainViewModel();
            BindingContext = _viewModel;

            // Fetch jobs initially
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

                if (string.IsNullOrEmpty(searchText))
                {
                    _viewModel.FilterItems(""); // Show all jobs if input is cleared
                    return;
                }

                var selectedRegion = RegionPicker.SelectedItem?.ToString() ?? "Alle";
                _viewModel.FilterItems(searchText, selectedRegion);
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

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = (SearchBar)sender;
            string searchText = searchBar.Text?.Trim();
            var selectedRegion = RegionPicker.SelectedItem?.ToString() ?? "Alle";

            if (!string.IsNullOrEmpty(searchText))
            {
                // Perform the search logic
                _viewModel.FilterItems(searchText, selectedRegion);

                // Add the search term to the history
                _viewModel.AddSearchTermToHistory(searchText);
            }

            // Clear the search bar without triggering TextChanged logic
            searchBar.TextChanged -= OnSearchTextChanged; // Temporarily detach TextChanged
            searchBar.Text = string.Empty;               // Clear the search bar
            searchBar.TextChanged += OnSearchTextChanged; // Reattach TextChanged
        }
    }
}
