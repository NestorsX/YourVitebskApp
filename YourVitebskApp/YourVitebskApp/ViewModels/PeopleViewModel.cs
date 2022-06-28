using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class PeopleViewModel : INotifyPropertyChanged
    {
        private ObservableRangeCollection<UsersListItem> _usersCollection;
        private IEnumerable<UsersListItem> _usersList;
        private int _currentOffset;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private bool _isLoadingMore;
        private readonly UserService _userService;
        public AsyncCommand PageAppearingCommand { get; set; }
        public AsyncCommand<UsersListItem> ItemTappedCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableRangeCollection<UsersListItem> UsersCollection
        {
            get { return _usersCollection; }
            set
            {
                _usersCollection = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<UsersListItem> UsersList
        {
            get { return _usersList; }
            set
            {
                _usersList = value;
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

        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged();
            }
        }

        public PeopleViewModel()
        {
            UsersCollection = new ObservableRangeCollection<UsersListItem>();
            _userService = new UserService();
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            ItemTappedCommand = new AsyncCommand<UsersListItem>(DialNumber);
            LoadMoreCommand = new Command(LoadMoreData);
            RefreshCommand = new Command(Refresh);
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
                try
                {
                    UsersCollection.Clear();
                    UsersList = await _userService.GetAll(Convert.ToInt32(await SecureStorage.GetAsync("UserId")));
                    foreach (var item in UsersList)
                    {
                        if (string.IsNullOrEmpty(item.PhoneNumber))
                        {
                            item.PhoneNumber = "Номер телефона не указан";
                        }
                    }

                    UsersCollection.AddRange(UsersList.Take(5));
                    _currentOffset = 5;
                }
                catch
                {

                }
            }
        }

        private void LoadMoreData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsLoadingMore = true;
                try
                {
                    UsersCollection.AddRange(UsersList.Skip(_currentOffset).Take(5));
                    OnPropertyChanged(nameof(UsersCollection));
                    _currentOffset += 5;
                }
                catch
                {

                }

                IsLoadingMore = false;
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

        public async Task DialNumber(UsersListItem sender)
        {
            if (!sender.PhoneNumber.Equals("Номер телефона не указан"))
            {
                await Task.Run(() => PhoneDialer.Open(sender.PhoneNumber));
            }
        }

        private async void Refresh()
        {
            IsRefreshing = true;
            await LoadData();
            IsRefreshing = false;
        }
    }
}
