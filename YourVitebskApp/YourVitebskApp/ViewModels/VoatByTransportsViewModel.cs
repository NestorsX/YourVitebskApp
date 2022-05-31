using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class VoatByTransportsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<VoatByTransport> _transportList;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly TransportSheduleService _transportSheduleService;
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<VoatByTransport> TransportList
        {
            get { return _transportList; }
            set
            {
                _transportList = value;
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
            IsBusy = true;
            _transportSheduleService = new TransportSheduleService();
            RefreshCommand = new Command(Refresh);
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
                try
                {
                    TransportList = (await _transportSheduleService.GetTransportsInfo()).data;
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

        private void Refresh()
        {
            IsRefreshing = true;
            AddData();
            IsRefreshing = false;
        }
    }
}
