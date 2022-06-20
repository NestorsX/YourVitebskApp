using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class RestorePasswordViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _firstName;
        private string _error;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isCompleteLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isError;
        private readonly AuthService _authService;
        public Command RestorePasswordCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
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

        public bool IsMainLayoutVisible
        {
            get { return _isMainLayoutVisible; }
            set
            {
                _isMainLayoutVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsCompleteLayoutVisible
        {
            get { return _isCompleteLayoutVisible; }
            set
            {
                _isCompleteLayoutVisible = value;
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

        public string DisplayMessage
        {
            get { return _error; }
        }

        public RestorePasswordViewModel()
        {
            IsBusy = true;
            _authService = new AuthService();
            RestorePasswordCommand = new Command(RestorePassword);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
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

        private async void RestorePassword()
        {
            IsBusy = true;
            try
            {
                await _authService.RestorePassword(Email, FirstName);
                IsBusy = false;
                IsMainLayoutVisible = false;
                IsCompleteLayoutVisible = true;
            }
            catch (ArgumentException e)
            {
                Error = e.Message;
                IsBusy = false;
            }
        }
    }
}
