using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Helpers;

namespace YourVitebskApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            Authorize();
        }

        private async void Authorize()
        {
            if (!string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("Token")))
            {
                await Shell.Current.GoToAsync("//Main");
            }
        }

        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            ThemeSwitcher.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            ThemeSwitcher.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ThemeSwitcher.SetTheme();
            });
        }
    }
}
