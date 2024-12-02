using System.Collections.ObjectModel;
using System.Linq;

public class MainViewModel
{
    // All items from the API
    public ObservableCollection<string> Items { get; set; }

    // Filtered items bound to the UI
    public ObservableCollection<string> FilteredItems { get; set; }

    public MainViewModel()
    {
        // Example data for initial display
        Items = new ObservableCollection<string>
        {
            "Butiksassistent",
            "Servicemedarbejder",
            "Avisbud",
            "Køkkenmedarbejder",
            "Receptionist"
        };

        // Initially show all items
        FilteredItems = new ObservableCollection<string>(Items);
    }

    // Method to filter items based on search text
    public void FilterItems(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Show all if no search text
            FilteredItems.Clear();
            foreach (var item in Items)
                FilteredItems.Add(item);
        }
        else
        {
            // Filter items
            var filtered = Items.Where(item => item.ToLower().Contains(searchText.ToLower())).ToList();

            FilteredItems.Clear();
            foreach (var item in filtered)
                FilteredItems.Add(item);
        }
    }
}
