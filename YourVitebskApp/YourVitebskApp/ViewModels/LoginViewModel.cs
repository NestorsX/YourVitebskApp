using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        private bool _isError;
        private AuthService _authService;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command LogInCommand { get; }
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

        public bool isMainLayoutVisible { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set 
            { 
                _isBusy = value;
                isMainLayoutVisible = !_isBusy;
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
            IsError = false;
            IsBusy = false;
        }

        private void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task Register()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
            IsBusy = false;
        }

        private async Task LogIn()
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
            }
            catch (ArgumentException e)
            {
                Error = e.Message;
            }

            IsBusy = false;
        }
    }
}
