using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1
{
    public class MainViewModel
    {
        public ObservableCollection<Job> Items { get; set; }
        public ObservableCollection<Job> FilteredItems { get; set; }
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
            foreach (var regionName in GeographyMapping.Values.Distinct())
            {
                RegionOptions.Add(regionName);
            }
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
        }

        private bool MatchesRegion(Job job, string region)
        {
            return job.Geography != null &&
                   job.Geography.Any(id => GeographyMapping.TryGetValue(id, out var mappedRegion) && mappedRegion == region);
        }
    }
}