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
    public class BusRoutesViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private IEnumerable<BusRoute> _busRoutesList;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly BusService _busService;
        public AsyncCommand<BusStop> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public int BusId { get; set; }

        public IEnumerable<BusRoute> BusRoutesList
        {
            get { return _busRoutesList; }
            set
            {
                _busRoutesList = value;
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
                IsBusy = !IsInternetNotConnected;
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

        public BusRoutesViewModel()
        {
            IsBusy = true;
            _busService = new BusService();
            ItemTappedCommand = new AsyncCommand<BusStop>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
        }

        private async void AddData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    BusRoutesList = await _busService.Get(BusId);
                }
                catch
                {

                }

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

        private async Task ItemTapped(BusStop busStop)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(BusShedulePage)}?BusId={BusId}&BusStopId={busStop.BusStopId}");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            AddData();
            IsRefreshing = false;
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.TryGetValue("BusId", out string param))
            {
                int.TryParse(param, out int id);
                BusId = id;
                AddData();
            }
        }
    }
}
