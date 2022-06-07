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
    internal class VoatByTransportRoutesViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private IEnumerable<VoatByRoutesData> _routesList;
        private IEnumerable<TransportStopSearchingModel> _searchingList;
        private string _transportName;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly TransportSheduleService _transportSheduleService;
        public AsyncCommand<VoatByTransportStop> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string TransportId { get; set; }
        public string TransportType { get; set; }

        public IEnumerable<VoatByRoutesData> RoutesList
        {
            get { return _routesList; }
            set
            {
                _routesList = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TransportStopSearchingModel> SearchingList
        {
            get { return _searchingList; }
            set
            {
                _searchingList = value;
                OnPropertyChanged();
            }
        }

        public string TransportName
        {
            get { return _transportName; }
            set
            {
                _transportName = value;
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

        public VoatByTransportRoutesViewModel()
        {
            IsBusy = true;
            _transportSheduleService = new TransportSheduleService();
            ItemTappedCommand = new AsyncCommand<VoatByTransportStop>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
        }

        private async void LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    RoutesList = await _transportSheduleService.GetTransportRoutes(TransportId);
                    TransportName = RoutesList.First().attributes.tr_n;
                    SearchingList = new List<TransportStopSearchingModel>();
                    foreach (var item in RoutesList)
                    {
                        string direction = item.attributes.napr_name;
                        foreach (var stop in item.attributes.ost)
                        {
                            SearchingList = SearchingList.Append(new TransportStopSearchingModel
                            {
                                TransportId = stop.vid_mar_n,
                                DirectionId = stop.napr_id,
                                TransportStopId = stop.ost_id,
                                TransportType = TransportType,
                                DirectionName = direction,
                                StopName = stop.ost_n
                            });
                        }
                    }
                }
                catch
                {

                }

                IsBusy = false;
            }
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadData();
            IsRefreshing = false;
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.TryGetValue("TransportType", out string param))
            {
                TransportType = param;
            }

            if (query.TryGetValue("TransportId", out param))
            {
                TransportId = param;
            }

            LoadData();
        }

        private async Task ItemTapped(VoatByTransportStop sender)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(VoatByTransportShedulePage)}?" +
                            $"TransportId={sender.vid_mar_n}&" +
                            $"DirectionId={sender.napr_id}&" +
                            $"StopId={sender.ost_id}&" +
                            $"TransportType={TransportType}");
            IsBusy = false;
        }
    }
}