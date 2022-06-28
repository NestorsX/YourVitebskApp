using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private ImageSource _imageSource;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _commentsCount;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private readonly UserService _userService;
        public AsyncCommand PageAppearingCommand { get; set; }
        public AsyncCommand UpdateCommand { get; }
        public AsyncCommand SettingsCommand { get; }
        public AsyncCommand ExitCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

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

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
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

        public string CommentsCount
        {
            get { return _commentsCount; }
            set
            {
                _commentsCount = value;
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

        public ProfileViewModel()
        {
            _userService = new UserService();
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            UpdateCommand = new AsyncCommand(Update);
            SettingsCommand = new AsyncCommand(Settings);
            ExitCommand = new AsyncCommand(Exit);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task OnAppearing()
        {
            IsBusy = true;
            await LoadData();
            IsBusy = false;
        }

        private async Task LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                FirstName = await SecureStorage.GetAsync("FirstName");
                LastName = await SecureStorage.GetAsync("LastName");
                Email = await SecureStorage.GetAsync("Email");
                PhoneNumber = await SecureStorage.GetAsync("PhoneNumber");
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    PhoneNumber = "Номер телефона не указан";
                }

                ImageSource = await SecureStorage.GetAsync("Image");
                CommentsCount = await _userService.GetCommentsCount(Convert.ToInt32(await SecureStorage.GetAsync("UserId")));
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
            await Shell.Current.GoToAsync($"{nameof(EditProfilePage)}");
        }

        private async Task Settings()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
        }

        private async Task Exit()
        {
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}
