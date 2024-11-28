namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            BindingContext = viewModel;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Opdater den filtrerede liste baseret på søgetekst
            viewModel.FilterItems(e.NewTextValue);
        }

    }

}
