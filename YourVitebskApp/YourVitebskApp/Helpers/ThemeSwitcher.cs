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
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                case 1:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                case 2:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
                default:
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;

            }

            var e = DependencyService.Get<IEnvironment>();
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                e?.SetStatusBarColorAsync(Color.FromHex("#0a1214"), false);
            }
            else
            {
                e?.SetStatusBarColorAsync(Color.FromHex("#ffffff"), true);
            }
        }
    }
}
