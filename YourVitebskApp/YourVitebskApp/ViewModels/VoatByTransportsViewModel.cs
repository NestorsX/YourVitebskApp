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
    public class VoatByTransportsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<VoatByTransportData> _transportList;
        private IEnumerable<TransportSearchingModel> _searchingList;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly TransportSheduleService _transportSheduleService;
        public AsyncCommand PageAppearingCommand { get; set; }
        public AsyncCommand<VoatByTransportItem> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<VoatByTransportData> TransportList
        {
            get { return _transportList; }
            set
            {
                _transportList = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TransportSearchingModel> SearchingList
        {
            get { return _searchingList; }
            set
            {
                _searchingList = value;
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

        public VoatByTransportsViewModel()
        {
            _transportSheduleService = new TransportSheduleService();
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            ItemTappedCommand = new AsyncCommand<VoatByTransportItem>(ItemTapped);
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
                    TransportList = await _transportSheduleService.GetTransportsInfo();
                    SearchingList = new List<TransportSearchingModel>();
                    foreach(var transportType in TransportList)
                    {
                        string type = transportType.attributes.vid_tr;
                        foreach (var transport in transportType.attributes.transpes)
                        {
                            SearchingList = SearchingList.Append(new TransportSearchingModel
                            {
                                TransportId = transport.mar_id,
                                TransportType = type,
                                TransportName = transport.tr_n
                            });
                        }
                    }
                }
                catch
                {

                }
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

        private async void Refresh()
        {
            IsRefreshing = true;
            await LoadData();
            IsRefreshing = false;
        }

        private async Task ItemTapped(VoatByTransportItem sender)
        {
            await Shell.Current.GoToAsync($"{nameof(VoatByTransportRoutesPage)}?TransportType={sender.vid_tr}&TransportId={sender.mar_id}");
        }
    }
}
