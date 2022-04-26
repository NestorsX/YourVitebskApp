using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace YourVitebskApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            switch (Preferences.Get("CurrentAppTheme", "Default"))
            {
                case "Default":
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                case "Dark":
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
                case "Light":
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                default:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
            }

            MainPage = new AppShell();
            Authorize();
        }

        private async void Authorize()
        {
            if (Application.Current.Properties.ContainsKey("CurrentUserID"))
            {
                await Shell.Current.GoToAsync("//Main");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
