using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CafesPage : ContentPage
    {
        public CafesPage()
        {
            InitializeComponent();
            BindingContext = new CafesViewModel();
        }
    }
}