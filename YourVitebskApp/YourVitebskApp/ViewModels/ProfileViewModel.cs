using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Helpers;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private IEnumerable<UsersListItem> _usersList;
        private ImageSource _imageSource;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private UserService _userService;
        public AsyncCommand PageAppearingCommand { get; set; }
        public AsyncCommand PageDisappearingCommand { get; set; }
        public AsyncCommand UpdateCommand { get; }
        public AsyncCommand SettingsCommand { get; }
        public AsyncCommand ExitCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<UsersListItem> UsersList
        {
            get { return _usersList; }
            set
            {
                _usersList = value;
                OnPropertyChanged();
            }
        }

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
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

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set 
            { 
                _phoneNumber = value; 
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

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ProfileViewModel()
        {
            IsBusy = true;
            _userService = new UserService();
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            PageDisappearingCommand = new AsyncCommand(OnDisappearing);
            UpdateCommand = new AsyncCommand(Update);
            SettingsCommand = new AsyncCommand(Settings);
            ExitCommand = new AsyncCommand(Exit);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            AddData();
        }

        private async Task OnDisappearing()
        {
            
        }

        private async void AddData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                FirstName = await SecureStorage.GetAsync("FirstName");
                LastName = await SecureStorage.GetAsync("LastName");
                PhoneNumber = await SecureStorage.GetAsync("PhoneNumber");
                ImageSource = await SecureStorage.GetAsync("Image");
                UsersList = await _userService.Get(Convert.ToInt32(await SecureStorage.GetAsync("UserId")), 0, 5);
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
            await Shell.Current.GoToAsync($"{nameof(EditProfilePage)}");
            IsBusy = false;
        }

        private async Task Settings()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
            IsBusy = false;
        }

        private async Task Exit()
        {
            IsBusy = true;
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Login");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            AddData();
            IsRefreshing = false;
        }
    }
}
