using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Helpers;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            switch (Settings.Theme)
            {
                case 0:
                    UseDeviceThemeSettings = true;
                    break;
                case 1:
                    UseLightMode = true;
                    break;
                case 2:
                    UseDarkMode = true;
                    break;
            }

            BindingContext = this;
        }

        private bool _useDarkMode;
        private bool _useLightMode;
        private bool _useDeviceThemeSettings;

        public bool UseDeviceThemeSettings
        {
            get { return _useDeviceThemeSettings; }
            set
            {
                _useDeviceThemeSettings = value;
                if (_useDeviceThemeSettings)
                {
                    UseDarkMode = UseLightMode = false;
                    Settings.Theme = 0;
                    ThemeSwitcher.SetTheme();
                }
            }
        }

        public bool UseLightMode
        {
            get { return _useLightMode; }
            set
            {
                _useLightMode = value;
                if (_useLightMode)
                {
                    UseDarkMode = UseDeviceThemeSettings = false;
                    Settings.Theme = 1;
                    ThemeSwitcher.SetTheme();
                }
            }
        }

        public bool UseDarkMode
        {
            get { return _useDarkMode; }
            set
            {
                _useDarkMode = value;
                if (_useDarkMode)
                {
                    UseLightMode = UseDeviceThemeSettings = false;
                    Settings.Theme = 2;
                    ThemeSwitcher.SetTheme();
                }
            }
        }
    }
}