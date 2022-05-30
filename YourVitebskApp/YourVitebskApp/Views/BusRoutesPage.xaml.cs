using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class BusRoutesPage : ContentPage
    {
        public BusRoutesPage()
        {
            InitializeComponent();
            BindingContext = new BusRoutesViewModel();
        }
    }
}