using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _repeatedPassword;
        private string _firstName;
        private string _lastName;
        private bool _isBusy;
        private bool _isError;
        private bool _isUnconfirmedPassword;
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
                IsError = false;
                IsUnconfirmedPassword = false;
                if (Password != RepeatedPassword)
                {
                    IsUnconfirmedPassword = true;
                }

                OnPropertyChanged();
            }
        }

        public string RepeatedPassword
        {
            get { return _repeatedPassword; }
            set
            {
                _repeatedPassword = value;
                IsError = false;
                IsUnconfirmedPassword = false;
                if (!Password.Equals(RepeatedPassword))
                {
                    IsUnconfirmedPassword = true;
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

        public bool isMainLayoutVisible { get; set; }

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

        public bool IsUnconfirmedPassword
        {
            get
            {
                return _isUnconfirmedPassword;
            }
            set
            {
                _isUnconfirmedPassword = value;
                OnPropertyChanged();
            }
        }

        public string DisplayMessage
        {
            get
            {
                return "Неверные данные!";
            }
        }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await Register());
            IsError = false;
            IsBusy = false;
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task Register()
        {
            IsBusy = true;
            var userService = new UserService();
            try
            {
                User user = new User
                {
                    UserId = null,
                    Email = Email,
                    Password = Password,
                    RoleId = 1,
                    UserDatum = new UserDatum
                    {
                        UserDataId = null,
                        UserId = null,
                        FirstName = FirstName,
                        SecondName = null,
                        LastName = LastName,
                        PhoneNumber = null
                    }
                };

                User currentUser = await userService.Add(user);
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
