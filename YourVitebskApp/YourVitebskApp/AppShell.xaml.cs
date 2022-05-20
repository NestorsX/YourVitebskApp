using Xamarin.Forms;
using YourVitebskApp.Views;

namespace YourVitebskApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SpecificNewsPage), typeof(SpecificNewsPage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(TransportShedulePage)}", typeof(TransportShedulePage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(PostersPage)}", typeof(PostersPage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(CafesPage)}", typeof(CafesPage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(VacanciesPage)}", typeof(VacanciesPage));
            Routing.RegisterRoute($"{nameof(CafesPage)}/{nameof(SpecificCafePage)}", typeof(SpecificCafePage));
        }
    }
}
