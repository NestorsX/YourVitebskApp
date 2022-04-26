using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _newPassword;
        private string _firstName;
        private string _secondName;
        private string _lastName;
        private string _phoneNumber;
        private bool _isBusy;
        private UserService _userService;
        public Command UpdateCommand { get; }
        public Command SettingsCommand { get; }
        public Command ExitCommand { get; }
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
                OnPropertyChanged(); 
            }
        }

        public string SecondName
        {
            get { return _secondName; }
            set 
            {
                _secondName = value;
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

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set 
            { 
                _phoneNumber = value; 
                OnPropertyChanged(); 
            }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set 
            { 
                _isBusy = value; 
                OnPropertyChanged(); 
            }
        }

        public ProfileViewModel()
        {
            IsBusy = true;
            UpdateCommand = new Command(async () => await Update());
            SettingsCommand = new Command(async () => await Settings());
            ExitCommand = new Command(async () => await Exit());
            _userService = new UserService();
            User currentUser = Task.Run(async () => await _userService.Get((int)Application.Current.Properties["CurrentUserID"])).Result;
            Email = currentUser.Email;
            FirstName = currentUser.UserDatum.FirstName;
            SecondName = currentUser.UserDatum.SecondName;
            LastName = currentUser.UserDatum.LastName;
            PhoneNumber = currentUser.UserDatum.PhoneNumber;
            IsBusy = false;
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task Update()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"//{nameof(EditProfilePage)}");
            IsBusy = false;
        }

        private async Task Settings()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"//{nameof(SettingsPage)}");
            IsBusy = false;
        }

        private async Task Exit()
        {
            IsBusy = true;
            Application.Current.Properties.Remove("CurrentUserID");
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Login");
            IsBusy = false;
        }
    }
}
