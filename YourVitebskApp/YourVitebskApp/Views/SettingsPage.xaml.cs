using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            switch (Preferences.Get("CurrentAppTheme", "Default"))
            {
                case "Default":
                    UseDeviceThemeSettings = true;
                    break;
                case "Dark":
                    UseDarkMode = true;
                    break;
                case "Light":
                    UseLightMode = true;
                    break;
                default:
                    UseDeviceThemeSettings = true;
                    break;
            }

            BindingContext = this;
        }

        private bool _useDarkMode;
        private bool _useLightMode;
        private bool _useDeviceThemeSettings;

        public bool UseDarkMode
        {
            get
            {
                return _useDarkMode;
            }
            set
            {
                _useDarkMode = value;
                if (_useDarkMode)
                {
                    UseLightMode = UseDeviceThemeSettings = false;
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    Preferences.Set("CurrentAppTheme", "Dark");
                }

            }
        }

        public bool UseLightMode
        {
            get
            {
                return _useLightMode;
            }
            set
            {
                _useLightMode = value;
                if (_useLightMode)
                {
                    UseDarkMode = UseDeviceThemeSettings = false;
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    Preferences.Set("CurrentAppTheme", "Light");
                }
            }
        }

        public bool UseDeviceThemeSettings
        {
            get
            {
                return _useDeviceThemeSettings;
            }
            set
            {
                _useDeviceThemeSettings =  value;
                if (_useDeviceThemeSettings)
                {
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    Preferences.Set("CurrentAppTheme", "Default");
                }
            }

        }
    }
}