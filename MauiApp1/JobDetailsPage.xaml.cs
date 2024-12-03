using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class JobDetailsPage : ContentPage
    {
        public JobDetailsPage(Job selectedJob)
        {
            InitializeComponent(); // Ensure this matches the x:Class
            BindingContext = selectedJob;
        }
    }
}
