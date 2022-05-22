using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificNewsPage : ContentPage
    {
        public SpecificNewsPage()
        {
            InitializeComponent();
            BindingContext = new SpecificNewsViewModel();
        }
    }
}