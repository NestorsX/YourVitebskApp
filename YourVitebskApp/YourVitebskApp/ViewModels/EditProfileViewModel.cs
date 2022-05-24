using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class EditProfileViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _oldPassword;
        private string _newPassword;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _error;
        private string _isVisible;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isError;
        private bool _isSuccess;
        private AuthService _authService;
        public AsyncCommand UpdateCommand { get; }
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

        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                OnPropertyChanged();
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                if (FirstName.Length == 0)
                {
                    Error = "Заполните поле \"Имя\"";
                }

                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                if (LastName.Length == 0)
                {
                    Error = "Заполните поле \"Фамилия\"";
                }

                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                IsError = false;
                if (PhoneNumber.Length > 0)
                {
                    if (!Regex.IsMatch(PhoneNumber, @"^\+375\((33|29|44|25)\)(\d{3})\-(\d{2})\-(\d{2})"))
                    {
                        Error = "Неверный формат телефонного номера";
                    }
                }

                OnPropertyChanged();
            }
        }

        public string IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
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

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set
            {
                _isSuccess = value;
                OnPropertyChanged();
                IsMainLayoutVisible = !IsSuccess;
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

        public EditProfileViewModel()
        {
            IsBusy = true;
            _authService = new AuthService();
            UpdateCommand = new AsyncCommand(Update);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            AddData();
            IsBusy = false;
        }

        private async void AddData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                Email = await SecureStorage.GetAsync("Email");
                FirstName = await SecureStorage.GetAsync("FirstName");
                LastName = await SecureStorage.GetAsync("LastName");
                PhoneNumber = await SecureStorage.GetAsync("PhoneNumber");
                IsVisible = Convert.ToBoolean(Task.Run(async () => await SecureStorage.GetAsync("IsVisible")).Result) == true ? "Да" : "Нет";
                IsBusy = false;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task Update()
        {
            IsBusy = true;
            try
            {
                string token = await _authService.Update(new Models.User
                {
                    UserId = Convert.ToInt32(await SecureStorage.GetAsync("UserId")),
                    OldPassword = OldPassword,
                    NewPassword = NewPassword,
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    IsVisible = IsVisible == "Да"
                });

                _authService.SaveUserCreds(token);
                AddData();
            }
            catch (ArgumentException e)
            {
                Error = e.Message;
            }

            IsBusy = false;
        }
    }
}
