using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesPage : ContentPage
    {
        public ServicesPage()
        {
            InitializeComponent();
            BindingContext = new ServicesViewModel();
        }
    }
}