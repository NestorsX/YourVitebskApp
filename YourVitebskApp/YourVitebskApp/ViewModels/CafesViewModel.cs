using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class CafesViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Cafe> _cafesList;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly CafeService _cafesService;
        public AsyncCommand<Cafe> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Cafe> CafesList
        {
            get { return _cafesList; }
            set
            {
                _cafesList = value;
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

        public CafesViewModel()
        {
            IsBusy = true;
            _cafesService = new CafeService();
            ItemTappedCommand = new AsyncCommand<Cafe>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            AddData();
            IsBusy = false;
        }

        private async void AddData()
        {
            CafesList = await _cafesService.Get(0,5);
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task ItemTapped(Cafe cafe)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(CafesPage)}/{nameof(SpecificCafePage)}?CafeId={cafe.CafeId}");
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
