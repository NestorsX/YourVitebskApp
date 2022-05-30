using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusesPage : ContentPage
    {
        public BusesPage()
        {
            InitializeComponent();
            BindingContext = new BusesViewModel();
        }
    }
}