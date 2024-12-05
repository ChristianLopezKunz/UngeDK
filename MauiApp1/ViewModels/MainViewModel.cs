using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1
{
    public class MainViewModel
    {
        public ObservableCollection<Job> Items { get; set; }
        public ObservableCollection<Job> FilteredItems { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<Job>();
            FilteredItems = new ObservableCollection<Job>();
        }

        public void FilterItems(string searchText)
        {
            FilteredItems.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Show all items when search is empty
                foreach (var item in Items)
                {
                    FilteredItems.Add(item);
                }
                return;
            }

            // Filter items with case-insensitive search
            var filtered = Items.Where(item =>
                (!string.IsNullOrEmpty(item.JobTitle) &&
                 item.JobTitle.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrEmpty(item.CompanyName) &&
                 item.CompanyName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrEmpty(item.Resume) &&
                 item.Resume.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0))
            .ToList();

            foreach (var item in filtered)
            {
                FilteredItems.Add(item);
            }
        }
    }
}
