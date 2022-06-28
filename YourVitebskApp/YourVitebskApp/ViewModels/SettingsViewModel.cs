using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using YourVitebskApp.Helpers;

namespace YourVitebskApp.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private bool _useDarkMode;
        private bool _useLightMode;
        private bool _useDeviceThemeSettings;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        public event PropertyChangedEventHandler PropertyChanged;
        public AsyncCommand<string> TapCommand { get; set; }

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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                IsMainLayoutVisible = !_isBusy;
                OnPropertyChanged();
            }
        }

        public bool IsMainLayoutVisible
        {
            get { return _isMainLayoutVisible; }
            set
            {
                _isMainLayoutVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsInternetNotConnected
        {
            get { return _isInternetNotConnected; }
            set
            {
                _isInternetNotConnected = value;
                OnPropertyChanged();
                IsMainLayoutVisible = !IsInternetNotConnected;
            }
        }

        public SettingsViewModel()
        {
            switch (Settings.Theme)
            {
                case 0:
                    _useDeviceThemeSettings = true;
                    break;
                case 1:
                    _useLightMode = true;
                    break;
                case 2:
                    _useDarkMode = true;
                    break;
            }

            TapCommand = new AsyncCommand<string>(OpenURL);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task OpenURL(string url)
        {
            await Browser.OpenAsync(url, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show
            });
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }
    }
}
