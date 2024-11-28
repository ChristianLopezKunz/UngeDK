using System.Collections.ObjectModel;
using System.Linq;

public class MainViewModel
{
    public ObservableCollection<string> Items { get; set; }
    public ObservableCollection<string> FilteredItems { get; set; }

    public MainViewModel()
    {
        // Opret eksempeldata
        Items = new ObservableCollection<string>
        {
            "Butiksassistent",
            "Servicemedarbejder",
            "Avisbud",
            "Køkkenmedarbejder",
            "Receptionist"
        };

        // Start med at vise alle elementer
        FilteredItems = new ObservableCollection<string>(Items);
    }

    // Filtreringslogik
    public void FilterItems(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Vis alle, hvis der ikke søges
            FilteredItems.Clear();
            foreach (var item in Items)
                FilteredItems.Add(item);
        }
        else
        {
            // Filtrer baseret på søgetekst
            var filtered = Items.Where(item => item.ToLower().Contains(searchText.ToLower())).ToList();

            FilteredItems.Clear();
            foreach (var item in filtered)
                FilteredItems.Add(item);
        }
    }
}
