using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    internal class VoatByTransportSheduleViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private IEnumerable<VoatBySheduleData> _workingDaySheduleList;
        private IEnumerable<VoatBySheduleData> _dayOffSheduleList;
        private VoatBySheduleAttributeInfo _workingDayShedule;
        private VoatBySheduleAttributeInfo _dayOffShedule;
        private string _workingDayTime;
        private string _dayOffTime;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private readonly TransportSheduleService _transportSheduleService;
        public event PropertyChangedEventHandler PropertyChanged;
        public string TransportId { get; set; }
        public string DirectionId { get; set; }
        public string StopId { get; set; }
        public string TransportType { get; set; }

        public VoatBySheduleAttributeInfo WorkingDayShedule
        {
            get { return _workingDayShedule; }
            set
            {
                _workingDayShedule = value;
                OnPropertyChanged();
            }
        }

        public VoatBySheduleAttributeInfo DayOffShedule
        {
            get { return _dayOffShedule; }
            set
            {
                _dayOffShedule = value;
                OnPropertyChanged();
            }
        }

        public string WorkingDayTime
        {
            get { return _workingDayTime; }
            set
            {
                _workingDayTime = value;
                OnPropertyChanged();
            }
        }

        public string DayOffTime
        {
            get { return _dayOffTime; }
            set
            {
                _dayOffTime = value;
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

        public VoatByTransportSheduleViewModel()
        {
            IsBusy = true;
            _transportSheduleService = new TransportSheduleService();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
        }

        private async Task LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    _workingDaySheduleList = await _transportSheduleService.GetTransportShedule(TransportId, DirectionId, StopId, "1", TransportType);
                }
                catch
                {

                }

                try
                {
                    _dayOffSheduleList = await _transportSheduleService.GetTransportShedule(TransportId, DirectionId, StopId, "6", TransportType);
                }
                catch
                {

                }

                var timeString = new StringBuilder("");
                if (_workingDaySheduleList != null)
                {
                    WorkingDayShedule = _workingDaySheduleList.First().attributes.data_tr.First();
                    foreach (var item in WorkingDayShedule.times)
                    {
                        item.tm_hour = item.tm_hour.Length > 1 ? item.tm_hour : $"0{item.tm_hour}";
                        item.tm_minute = item.tm_minute.Length > 1 ? item.tm_minute : $"0{item.tm_minute}";
                        timeString.Append($"{item.tm_hour}:{item.tm_minute} ");
                    }

                    WorkingDayTime = timeString.ToString();
                }

                timeString.Clear();
                if (_dayOffSheduleList != null)
                {
                    DayOffShedule = _dayOffSheduleList.First().attributes.data_tr.First();
                    foreach (var item in DayOffShedule.times)
                    {
                        item.tm_hour = item.tm_hour.Length > 1 ? item.tm_hour : $"0{item.tm_hour}";
                        item.tm_minute = item.tm_minute.Length > 1 ? item.tm_minute : $"0{item.tm_minute}";
                        timeString.Append($"{item.tm_hour}:{item.tm_minute} ");
                    }

                    DayOffTime = timeString.ToString();
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

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            IsBusy = true;
            if (query.TryGetValue("TransportId", out string param))
            {
                TransportId = param;
            }

            if (query.TryGetValue("DirectionId", out param))
            {
                DirectionId = param;
            }

            if (query.TryGetValue("StopId", out param))
            {
                StopId = param;
            }

            if (query.TryGetValue("TransportType", out param))
            {
                TransportType = param;
            }

            await LoadData();
            IsBusy = false;
        }
    }
}
