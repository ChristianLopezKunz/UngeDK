using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace MauiApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Job> Items { get; set; }
        // Filtered collection of job items that match the search and filter criteria
        private ObservableCollection<Job> _filteredItems;
        private ObservableCollection<string> _searchHistory = new();

        public ObservableCollection<Job> FilteredItems
        {
            get => _filteredItems;
            set
            {
                // Update the value of FilteredItems
                _filteredItems = value;

                // Notify the UI that FilteredItems has changed
                OnPropertyChanged(nameof(FilteredItems));

                // Notify the UI that HasResults and IsNoResults should be updated
                OnPropertyChanged(nameof(HasResults));
                OnPropertyChanged(nameof(IsNoResults));
            }
        }

        public ObservableCollection<string> SearchHistory
        {
            get => _searchHistory;
            set
            {
                _searchHistory = value;
                OnPropertyChanged(nameof(SearchHistory));
            }
        }

        private string _selectedSearchTerm;
        public string SelectedSearchTerm
        {
            get => _selectedSearchTerm;
            set
            {
                if (_selectedSearchTerm != value)
                {
                    _selectedSearchTerm = value;
                    OnPropertyChanged(nameof(SelectedSearchTerm));

                    // Trigger the search with the selected term
                    if (!string.IsNullOrEmpty(value))
                        FilterItems(value);
                }
            }
        }

        // Indicates if there are results to display
        public bool HasResults => FilteredItems.Any();

        // Indicates if there are no results to display
        public bool IsNoResults => !HasResults;
        public ObservableCollection<string> RegionOptions { get; set; }

        private readonly Dictionary<int, string> GeographyMapping = new()
        {
            { 2, "Storkøbenhavn" },
            { 3, "Nordsjælland" },
            { 14, "Østsjælland" },
            { 4, "Vestsjælland" },
            { 5, "Sydsjælland & Øer" },
            { 13, "Fyn" },
            { 12, "Sønderjylland" },
            { 11, "Sydvestjylland (Esbjerg)" },
            { 9, "Vestjylland" },
            { 10, "Sydøstjylland" },
            { 7, "Midtjylland" },
            { 6, "Østjylland (Aarhus)" },
            { 8, "Nordjylland" },
            { 20, "Bornholm" }
        };


        public MainViewModel()
        {
            Items = new ObservableCollection<Job>();
            FilteredItems = new ObservableCollection<Job>();
            RegionOptions = new ObservableCollection<string> { "Alle" };
            SearchHistory = new ObservableCollection<string>();
            foreach (var regionName in GeographyMapping.Values.Distinct())
            {
                RegionOptions.Add(regionName);
            }

            LoadSearchHistory();
        }

        public void FilterItems(string searchText, string selectedRegion = "Alle")
        {
            FilteredItems.Clear();

            var filtered = Items.Where(item =>
                // Match search text
                (string.IsNullOrEmpty(searchText) ||
                 (!string.IsNullOrEmpty(item.JobTitle) && item.JobTitle.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 (!string.IsNullOrEmpty(item.CompanyName) && item.CompanyName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 (!string.IsNullOrEmpty(item.GeographyDisplay) && item.GeographyDisplay.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                // Match selected region
                (selectedRegion == "Alle" || MatchesRegion(item, selectedRegion))
            ).ToList();

            foreach (var item in filtered)
            {
                FilteredItems.Add(item);
            }

            // Notify the UI that HasResults and IsNoResults may have changed
            OnPropertyChanged(nameof(HasResults));
            OnPropertyChanged(nameof(IsNoResults));
        }

        private bool MatchesRegion(Job job, string region)
        {
            return job.Geography != null &&
                   job.Geography.Any(id => GeographyMapping.TryGetValue(id, out var mappedRegion) && mappedRegion == region);
        }

        // Event to notify UI about property changes
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the UI when a property changes.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddSearchTermToHistory(string term)
        {
            if (!SearchHistory.Contains(term))
            {
                SearchHistory.Insert(0, term);

                if (SearchHistory.Count > 5)
                {
                    SearchHistory.RemoveAt(SearchHistory.Count - 1);
                }

                SaveSearchHistory(); // Save updated history
            }
        }

        public void SaveSearchHistory()
        {
            var historyJson = JsonSerializer.Serialize(SearchHistory);
            Preferences.Set("SearchHistory", historyJson);
        }

        public void LoadSearchHistory()
        {
            if (Preferences.ContainsKey("SearchHistory"))
            {
                var historyJson = Preferences.Get("SearchHistory", string.Empty);
                if (!string.IsNullOrEmpty(historyJson))
                {
                    SearchHistory = JsonSerializer.Deserialize<ObservableCollection<string>>(historyJson) ?? new ObservableCollection<string>();
                }
            }
        }
    }
}