using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoatByTransportsPage : ContentPage
    {
        public VoatByTransportsPage()
        {
            InitializeComponent();
            BindingContext = new VoatByTransportsViewModel();
        }
    }
}