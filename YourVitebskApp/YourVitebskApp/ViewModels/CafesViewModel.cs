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
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class CafesViewModel : INotifyPropertyChanged
    {
        private ObservableRangeCollection<Cafe> _cafesCollection;
        private IEnumerable<Cafe> _cafesList;
        private int _currentOffset;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private bool _isLoadingMore;
        private readonly CafeService _cafesService;
        public AsyncCommand<Cafe> ItemTappedCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableRangeCollection<Cafe> CafesCollection
        {
            get { return _cafesCollection; }
            set
            {
                _cafesCollection = value;
                OnPropertyChanged();
            }
        }

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

        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged();
            }
        }

        public CafesViewModel()
        {
            CafesCollection = new ObservableRangeCollection<Cafe>();
            _cafesService = new CafeService();
            ItemTappedCommand = new AsyncCommand<Cafe>(ItemTapped);
            LoadMoreCommand = new Command(LoadMoreData);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            LoadData();
        }

        private async void LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    CafesCollection.Clear();
                    CafesList = await _cafesService.GetAll();
                    CafesCollection.AddRange(CafesList.Take(5));
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
                    CafesCollection.AddRange(CafesList.Skip(_currentOffset).Take(5));
                    OnPropertyChanged(nameof(CafesCollection));
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

        private async Task ItemTapped(Cafe cafe)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(CafesPage)}/{nameof(SpecificCafePage)}?CafeId={cafe.CafeId}");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadData();
            IsRefreshing = false;
        }
    }
}
