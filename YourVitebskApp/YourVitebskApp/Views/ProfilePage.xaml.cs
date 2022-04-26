using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            Routing.RegisterRoute("//SettingsPage", typeof(SettingsPage));
            Routing.RegisterRoute("//EditProfilePage", typeof(EditProfilePage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new ProfileViewModel();
        }
    }
}