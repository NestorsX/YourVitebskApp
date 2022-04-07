using MyVitebskApp.Models;
using MyVitebskApp.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyVitebskApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            SetupCurrentTheme();
        }

        public void SetupCurrentTheme()
        {
            var currentTheme = Preferences.Get("CurrentAppTheme", null);
            if (currentTheme != null)
            {
                if (Enum.TryParse(currentTheme, out UITheme currentThemeEnum))
                {
                    ThemeSwitcherService.SetAppTheme(currentThemeEnum);
                }
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
