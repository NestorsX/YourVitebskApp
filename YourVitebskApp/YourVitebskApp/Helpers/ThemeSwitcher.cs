using Xamarin.Forms;

namespace YourVitebskApp.Helpers
{
    public static class ThemeSwitcher
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                case 0:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                case 1:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                case 2:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }

            var e = DependencyService.Get<IEnvironment>();
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                e?.SetStatusBarColorAsync(Color.FromHex("#373737"), false);
            }
            else
            {
                e?.SetStatusBarColorAsync(Color.FromHex("#fafafa"), true);
            }
        }
    }
}
