using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _error;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isError;
        private readonly AuthService _authService;
        public Command LogInCommand { get; }
        public Command RegisterCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get { return _email; }
            set 
            { 
                _email = value;
                IsError = false;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                IsError = false;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                IsError = true;
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

        public bool IsError
        {
            get { return _isError; }
            set 
            {
                _isError = value;
                OnPropertyChanged();
                if (IsError)
                {
                    OnPropertyChanged(nameof(DisplayMessage));
                }
            }
        }

        public string DisplayMessage
        {
            get { return _error; }
        }

        public LoginViewModel()
        {
            IsBusy = true;
            _authService = new AuthService();
            LogInCommand = new Command(async () => await LogIn());
            RegisterCommand = new Command(async () => await Register());
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsBusy = false;
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task Register()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
            IsBusy = false;
        }

        private async Task LogIn()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    string token = await _authService.Login(new UserLoginDTO
                    {
                        Email = Email,
                        Password = Password,
                    });

                    _authService.SaveUserCreds(token);
                    await Shell.Current.GoToAsync("//Main");
                    IsBusy = false;
                }
                catch (ArgumentException e)
                {
                    Error = e.Message;
                }

                IsBusy = false;
            }
        }
    }
}
