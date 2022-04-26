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
        private bool _isBusy;
        private bool _isError;
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

        public bool isMainLayoutVisible { get; set; }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set 
            { 
                _isBusy = value;
                isMainLayoutVisible = !_isBusy;
                OnPropertyChanged();
            }
        }

        public bool IsError
        {
            get
            {
                return _isError;
            }
            set 
            {
                _isError = value;
                OnPropertyChanged();
            }
        }

        public string DisplayMessage
        {
            get
            {
                return "Неверные логин и/или пароль!";
            }
        }

        public LoginViewModel()
        {
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
            var userService = new UserService();
            try
            {
                User currentUser = await userService.Get(Email, Password);
                Application.Current.Properties["CurrentUserID"] = currentUser.UserId;
                await Shell.Current.GoToAsync("//Main");
            }
            catch
            {
                IsError = true;
            }

            IsBusy = false;
        }
    }
}
