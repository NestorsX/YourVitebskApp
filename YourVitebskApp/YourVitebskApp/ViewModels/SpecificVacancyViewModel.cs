using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    public class SpecificVacancyViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private Vacancy _vacancy;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private readonly VacancyService _vacancyService;
        public event PropertyChangedEventHandler PropertyChanged;
        public int VacancyId { get; set; }

        public Vacancy Vacancy
        {
            get { return _vacancy; }
            set
            {
                _vacancy = value;
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

        public SpecificVacancyViewModel()
        {
            _vacancyService = new VacancyService();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private async Task LoadData()
        {
            Vacancy = await _vacancyService.Get(VacancyId);
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
            if (query.TryGetValue("VacancyId", out string param))
            {
                int.TryParse(param, out int id);
                VacancyId = id;
                await LoadData();
            }

            IsBusy = false;
        }
    }
}
