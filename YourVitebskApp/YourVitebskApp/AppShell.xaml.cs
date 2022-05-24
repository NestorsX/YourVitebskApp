using Xamarin.Forms;
using YourVitebskApp.Views;

namespace YourVitebskApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(SpecificNewsPage), typeof(SpecificNewsPage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(TransportShedulePage)}", typeof(TransportShedulePage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(PostersPage)}", typeof(PostersPage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(CafesPage)}", typeof(CafesPage));
            Routing.RegisterRoute($"{nameof(ServicesPage)}/{nameof(VacanciesPage)}", typeof(VacanciesPage));
            Routing.RegisterRoute($"{nameof(PostersPage)}/{nameof(SpecificPosterPage)}", typeof(SpecificPosterPage));
            Routing.RegisterRoute($"{nameof(CafesPage)}/{nameof(SpecificCafePage)}", typeof(SpecificCafePage));
            Routing.RegisterRoute($"{nameof(VacanciesPage)}/{nameof(SpecificVacancyPage)}", typeof(SpecificVacancyPage));
        }
    }
}
