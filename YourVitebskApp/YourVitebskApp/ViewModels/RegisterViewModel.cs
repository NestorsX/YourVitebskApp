using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _error;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isError;
        private readonly AuthService _authService;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command RegisterCommand { get; }

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
                Error = "Пароль не должен быть короче 6 символов";
                if (Password.Length >= 6)
                {
                    IsError = false;
                }

                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
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

        public RegisterViewModel()
        {
            IsBusy = true;
            _authService = new AuthService();
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
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    var user = new UserRegisterDTO
                    {
                        Email = Email,
                        Password = Password,
                        FirstName = FirstName,
                        LastName = LastName
                    };

                    string token = await _authService.Register(user);
                    _authService.SaveUserCreds(token);
                    await Shell.Current.GoToAsync("//Main");
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
