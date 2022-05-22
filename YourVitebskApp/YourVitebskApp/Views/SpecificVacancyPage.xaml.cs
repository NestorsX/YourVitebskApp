using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificVacancyPage : ContentPage
    {
        public SpecificVacancyPage()
        {
            InitializeComponent();
            BindingContext = new SpecificVacancyViewModel();
        }
    }
}