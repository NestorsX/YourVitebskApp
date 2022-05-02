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
            Routing.RegisterRoute($"//{nameof(TransportShedulePage)}", typeof(TransportShedulePage));
            Routing.RegisterRoute($"//{nameof(PostersPage)}", typeof(PostersPage));
            Routing.RegisterRoute($"//{nameof(CafesPage)}", typeof(CafesPage));
            Routing.RegisterRoute($"//{nameof(VacanciesPage)}", typeof(VacanciesPage));
            BindingContext = new ServicesViewModel();
        }
    }
}