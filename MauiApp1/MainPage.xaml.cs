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

                // Require at least 3 characters before triggering local filtering
                if (string.IsNullOrEmpty(searchText) || searchText.Length < 3)
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
            string searchText = SearchBarControl.Text?.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Add the search term to history
                _viewModel.AddSearchTermToHistory(searchText);

                // Perform filtering
                var selectedRegion = RegionPicker.SelectedItem?.ToString() ?? "Alle";
                _viewModel.FilterItems(searchText, selectedRegion);
            }
        }
    }
}
