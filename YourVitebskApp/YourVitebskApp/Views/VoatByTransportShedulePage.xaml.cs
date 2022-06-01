using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoatByTransportShedulePage : ContentPage
    {
        public VoatByTransportShedulePage()
        {
            InitializeComponent();
            BindingContext = new VoatByTransportSheduleViewModel();
        }
    }
}