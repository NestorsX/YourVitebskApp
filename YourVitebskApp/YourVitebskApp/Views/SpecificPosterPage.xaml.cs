using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificPosterPage : ContentPage
    {
        public SpecificPosterPage()
        {
            InitializeComponent();
            BindingContext = new SpecificPosterViewModel();
        }
    }
}