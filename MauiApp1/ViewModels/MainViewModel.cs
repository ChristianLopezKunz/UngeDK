using System.Collections.ObjectModel;
using System.Linq;

public class MainViewModel
{
    // Fulde liste af jobs fra API'et
    public ObservableCollection<string> Items { get; set; }

    // Filtrerede resultater, der vises i UI
    public ObservableCollection<string> FilteredItems { get; set; }

    public MainViewModel()
    {
        Items = new ObservableCollection<string>();
        FilteredItems = new ObservableCollection<string>();
    }

    // Lokal filtrering baseret på søgetekst
    public void FilterItems(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Vis alle jobs, hvis søgeteksten er tom
            FilteredItems.Clear();
            foreach (var item in Items)
                FilteredItems.Add(item);
        }
        else
        {
            // Delvise matches (case-insensitive)
            var filtered = Items
                .Where(item => item.ToLower().Contains(searchText.ToLower()))
                .ToList();

            FilteredItems.Clear();
            foreach (var item in filtered)
                FilteredItems.Add(item);
        }
    }
}
