using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VacanciesPage : ContentPage
    {
        public VacanciesPage()
        {
            InitializeComponent();
            BindingContext = new VacanciesViewModel();
        }
    }
}